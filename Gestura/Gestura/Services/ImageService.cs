using Gestura.Commons;
using Gestura.Interfaces;
using Gestura.Models;
using SkiaSharp;

namespace Gestura.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageDirectoryService _directoryService;
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _directoryService = MauiProgram.Services.GetService<IImageDirectoryService>();
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));

            if (!Directory.Exists(Constantes.ImageFolderPath))
            {
                Directory.CreateDirectory(Constantes.ImageFolderPath);
            }
        }

        public async Task<ImageReference> ImportImageFromLocalAsync(string directoryName = null)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Sélectionner une image",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null)
            {
                throw new InvalidDataException("Erreur lors de la récupération du fichier dans le stockage local.");
            }

            if (string.IsNullOrWhiteSpace(directoryName))
            {
                directoryName = Constantes.DEFAULT_FROM_LOCALSTORAGE_DIRECTORY;
            }

            var directory = await _directoryService.GetDirectoryByNameAsync(directoryName);
            if (directory == null)
            {
                var newDirectory = new Models.ImageDirectory { Name = directoryName };
                var success = await _directoryService.CreateDirectoryAsync(newDirectory);
                if (/*!success*/success is null)
                {
                    throw new InvalidDataException($"Erreur lors de la création du répertoire {directoryName}.");
                }
                else
                {
                    directory = newDirectory;
                }
            }

            var directoryPath = Path.Combine(Constantes.ImageFolderPath, directory.Name);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = await result.OpenReadAsync())
            {
                var filePath = Path.Combine(directoryPath, result.FileName);

                if (File.Exists(filePath))
                {
                    throw new InvalidDataException("Le fichier existe déjà.");
                }

                await SaveImageFileAsync(filePath, stream);

                var imageReference = new ImageReference
                {
                    FileName = result.FileName,
                    FilePath = filePath,
                    CreatedAt = DateTime.Now,
                    DirectoryPath = directoryPath,
                    DirectoryId = directory.Id,
                };

                var saveResult = await _imageRepository.SaveImageAsync(imageReference);
                if (saveResult > 0)
                {
                    return imageReference;
                }
                else
                {
                    File.Delete(filePath);
                    throw new InvalidDataException("Échec de la sauvegarde de l'image en base de données.");
                }
            }
        }

        public async Task<ImageReference> ImportImageFromUrlAsync(string imageUrl, string directoryName = null)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentNullException(nameof(imageUrl));
            }

            if (string.IsNullOrWhiteSpace(directoryName))
            {
                directoryName = Constantes.DEFAULT_FROM_WEBURL_DIRECTORY;
            }

            var directory = await _directoryService.GetDirectoryByNameAsync(directoryName);
            if (directory == null)
            {
                var newDirectory = new Models.ImageDirectory { Name = directoryName };
                var success = await _directoryService.CreateDirectoryAsync(newDirectory);
                if (/*!success*/success is not null)
                {
                    throw new InvalidDataException($"Erreur lors de la création du répertoire {directoryName}.");
                }
                else
                {
                    directory = newDirectory;
                }
            }

            var directoryPath = Path.Combine(Constantes.ImageFolderPath, directory.Name);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(imageUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"Échec du téléchargement de l'image depuis l'URL : {imageUrl}");
                }

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var fileName = Path.GetFileName(new Uri(imageUrl).LocalPath);
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        fileName = Guid.NewGuid().ToString().ToUpperInvariant() + ".jpg";
                    }
                    var filePath = Path.Combine(Constantes.ImageFolderPath, fileName);

                    if (File.Exists(filePath))
                    {
                        throw new InvalidDataException("Le fichier existe déjà.");
                    }

                    await SaveImageFileAsync(filePath, stream);

                    var imageReference = new ImageReference
                    {
                        FileName = fileName,
                        FilePath = filePath,
                        CreatedAt = DateTime.Now,
                        DirectoryPath = directoryPath,
                        DirectoryId = directory.Id
                    };

                    var saveResult = await _imageRepository.SaveImageAsync(imageReference);
                    if (saveResult > 0)
                    {
                        return imageReference;
                    }
                    else
                    {
                        File.Delete(filePath);
                        throw new InvalidDataException("Échec de la sauvegarde de l'image en base de données.");
                    }
                }
            }
        }

        private async Task SaveImageFileAsync(string filePath, Stream stream)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Le chemin du fichier ne peut pas être vide.", nameof(filePath));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream), "Le flux d'entrée ne peut pas être nul.");
            }

            if (!stream.CanRead)
            {
                throw new ArgumentException("Le flux d'entrée doit être lisible.", nameof(stream));
            }

            // S'assurer que le répertoire existe
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // effectuer le traitement des images sans bloquer le thread principal
            await Task.Run(async () =>
            {
                var resizedStream = await ResizeImageAsync(stream);

                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await resizedStream.CopyToAsync(fileStream);
                }
            });
        }

        public async Task<List<ImageReference>> GetAllImagesAsync()
        {
            return await _imageRepository.GetAllImagesAsync();
        }

        public async Task<List<ImageReference>> GetImagesByDirectoryIdAsync(int directoryId)
        {
            return await _imageRepository.GetImagesByDirectoryIdAsync(directoryId);
        }

        public async Task<bool> DeleteImageAsync(ImageReference image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (File.Exists(image.FilePath))
            {
                File.Delete(image.FilePath);
            }

            var deleteResult = await _imageRepository.DeleteImageAsync(image) > 0;
            return deleteResult;
        }

        private static async Task<Stream> ResizeImageAsync(Stream originalStream)
        {
            if (originalStream == null)
            {
                throw new ArgumentNullException(nameof(originalStream), "Le flux d'entrée ne peut pas être nul.");
            }

            if (!originalStream.CanRead)
            {
                throw new ArgumentException("Le flux d'entrée doit être lisible.", nameof(originalStream));
            }

            MemoryStream output;

            // Créer une copie du flux d'origine
            using var memoryStream = new MemoryStream();
            await originalStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            // Décoder l'image d'origine
            using var original = SKBitmap.Decode(memoryStream);
            if (original == null)
            {
                throw new InvalidOperationException("Impossible de décoder l'image d'origine.");
            }

            var originalWidth = original.Width;
            var originalHeight = original.Height;

            // Vérifier si le redimensionnement est nécessaire
            if (!ImageUtils.ShouldResize(originalWidth, originalHeight))
            {
                // Retourner une nouvelle copie du flux pour éviter les problèmes de flux fermé
                output = new MemoryStream(memoryStream.ToArray());
                output.Position = 0;
                return output;
            }

            // Calculer les nouvelles dimensions
            var (newWidth, newHeight) = ImageUtils.GetResizedDimensions(originalWidth, originalHeight);

            // Créer une nouvelle image redimensionnée
            var resized = new SKBitmap(newWidth, newHeight);
            var samplingOptions = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None);
            if (!original.ScalePixels(resized, samplingOptions))
            {
                throw new InvalidOperationException("Le redimensionnement de l'image a échoué.");
            }

            // Encoder l'image redimensionnée en JPEG
            using var image = SKImage.FromBitmap(resized);
            output = new MemoryStream();
            using var encodedData = image.Encode(SKEncodedImageFormat.Jpeg, 80);
            if (encodedData == null)
            {
                throw new InvalidOperationException("L'encodage de l'image a échoué.");
            }

            encodedData.SaveTo(output);
            output.Position = 0;
            return output;
        }
    }
}
