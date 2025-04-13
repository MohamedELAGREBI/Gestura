using Gestura.Models;

namespace Gestura.Interfaces
{
    public interface IImageDirectoryService
    {
        Task<ImageDirectory> CreateDirectoryAsync(ImageDirectory directory);
        Task<bool> DeleteDirectoryAsync(ImageDirectory directory);
        Task<ImageDirectory> GetDirectoryByIdAsync(int directoryId);
        Task<ImageDirectory> GetDirectoryByNameAsync(string directoryName);
        Task<List<ImageDirectory>> GetAllDirectoriesAsync();

        Task<List<ImageReference>> GetImagesForDirectoryAsync(int directoryId);
    }
}
