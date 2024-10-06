using Gestura.Commons;
using Gestura.Interfaces;
using Gestura.Models;

namespace Gestura.Services
{
    public class ImageService : IImageService
    {
        private readonly IDirectoryService _directoryService;
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _directoryService = MauiProgram.Services.GetService<IDirectoryService>();
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));

            if (!System.IO.Directory.Exists(Constantes.ImageFolderPath))
            {
                System.IO.Directory.CreateDirectory(Constantes.ImageFolderPath);
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
                var newDirectory = new Models.Directory { Name = directoryName };
                var success = await _directoryService.CreateDirectoryAsync(newDirectory);
                if (!success)
                {
                    throw new InvalidDataException($"Erreur lors de la création du répertoire {directoryName}.");
                }
                else
                {
                    directory = newDirectory;
                }
            }

            var directoryPath = Path.Combine(Constantes.ImageFolderPath, directory.Name);

            if (!System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.CreateDirectory(directoryPath);
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
                var newDirectory = new Models.Directory { Name = directoryName };
                var success = await _directoryService.CreateDirectoryAsync(newDirectory);
                if (!success)
                {
                    throw new InvalidDataException($"Erreur lors de la création du répertoire {directoryName}.");
                }
                else
                {
                    directory = newDirectory;
                }
            }

            var directoryPath = Path.Combine(Constantes.ImageFolderPath, directory.Name);

            if (!System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.CreateDirectory(directoryPath);
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
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
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


    }
}
