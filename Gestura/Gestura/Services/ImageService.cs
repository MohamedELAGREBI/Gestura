using Gestura.Interfaces;
using Gestura.Models;

namespace Gestura.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imageFolderPath;
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
            _imageFolderPath = Path.Combine(FileSystem.AppDataDirectory, "Images");

            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }

        public async Task<ImageReference> ImportImageFromLocalAsync()
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

            using (var stream = await result.OpenReadAsync())
            {
                var filePath = Path.Combine(_imageFolderPath, result.FileName);

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

        public async Task<ImageReference> ImportImageFromUrlAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentNullException(nameof(imageUrl));
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
                    var filePath = Path.Combine(_imageFolderPath, fileName);

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
