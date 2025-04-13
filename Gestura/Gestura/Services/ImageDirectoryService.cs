using Gestura.Interfaces;
using Gestura.Models;

namespace Gestura.Services
{
    public class DirectoryService : IImageDirectoryService
    {
        private readonly IImageDirectoryRepository _directoryRepository;
        private readonly IImageRepository _imageRepository;

        public DirectoryService(IImageDirectoryRepository directoryRepository, IImageRepository imageRepository)
        {
            _directoryRepository = directoryRepository;
            _imageRepository = imageRepository;
        }

        public async Task<ImageDirectory> CreateDirectoryAsync(ImageDirectory directory)
        {
            await _directoryRepository.InsertAsync(directory);
            return directory;
        }

        public async Task<bool> DeleteDirectoryAsync(ImageDirectory directory)
        {
            return await _directoryRepository.DeleteAsync(directory) > 0;
        }

        public async Task<Models.ImageDirectory> GetDirectoryByIdAsync(int directoryId)
        {
            return await _directoryRepository.GetDirectoryByIdAsync(directoryId);
        }

        public async Task<Models.ImageDirectory> GetDirectoryByNameAsync(string directoryName)
        {
            return await _directoryRepository.GetDirectoryByNameAsync(directoryName);
        }

        public async Task<List<Models.ImageDirectory>> GetAllDirectoriesAsync()
        {
            return await _directoryRepository.GetAllAsync();
        }

        public async Task<List<ImageReference>> GetImagesForDirectoryAsync(int directoryId)
        {
            return await _imageRepository.GetImagesByDirectoryIdAsync(directoryId);
        }
    }
}
