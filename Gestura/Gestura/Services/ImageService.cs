using Gestura.Interfaces;
using Gestura.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestura.Services
{
    public class ImageService
    {
        private readonly string _imageFolderPath;
        private readonly IImageRepository _imageRepository;

        public ImageService()
        {
            _imageRepository = MauiProgram.Services.GetService<IImageRepository>();
            _imageFolderPath = Path.Combine(FileSystem.AppDataDirectory, "Images");

            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }

        //var filePath = "https://i.pinimg.com/564x/91/2d/6f/912d6f086b9080aba5706fc98ce6e9ba.jpg";

        public async Task<ImageReference> ImportImageFromLocalAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Sélectionner une image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                        var filePath = Path.Combine(_imageFolderPath, result.FileName);
                        await SaveImageFileAsync(filePath, stream);

                        // Création de l'objet ImageReference et sauvegarde dans SQLite
                        var imageReference = new ImageReference
                        {
                            FileName = result.FileName,
                            FilePath = filePath,
                            CreatedAt = DateTime.Now,
                        };

                        await _imageRepository.SaveImageAsync(imageReference);
                        return imageReference;
                    }
                }
            }
            catch (Exception ex)
            {
                // message = $"Erreur lors de l'importation de l'image : {ex.Message}"
                throw;
            }

            return null;
        }

        public async Task<ImageReference> ImportImageFromUrlAsync(string imageUrl)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(imageUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            var fileName = Path.GetFileName(new Uri(imageUrl).LocalPath);
                            if (string.IsNullOrWhiteSpace(fileName))
                            {
                                fileName = Guid.NewGuid().ToString().ToUpperInvariant() + ".jpg";
                            }
                            var filePath = Path.Combine(_imageFolderPath, fileName);
                            await SaveImageFileAsync(filePath, stream);

                            var imageReference = new ImageReference
                            {
                                FileName = fileName,
                                FilePath = filePath,
                                CreatedAt = DateTime.Now,
                            };

                            await _imageRepository.SaveImageAsync(imageReference);
                            return imageReference;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // message = $"Erreur lors du téléchargement de l'image : {ex.Message}"
                throw;
            }

            return null;
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
            var images = new List<ImageReference>();
            if (Directory.Exists(_imageFolderPath))
            {
                var files = Directory.GetFiles(_imageFolderPath);
                foreach (var file in files)
                {
                    images.Add(new ImageReference { FileName = Guid.NewGuid().ToString().ToUpperInvariant() + ".jpg", FilePath = file });
                }
            }

            return await Task.FromResult(images);
        }

        public async Task<bool> DeleteImageAsync(ImageReference image)
        {
            if (image != null)
            {
                if (File.Exists(image.FilePath))
                {
                    File.Delete(image.FilePath);
                }

                await _imageRepository.DeleteImageAsync(image);
                return true;
            }

            return false;
        }


    }
}
