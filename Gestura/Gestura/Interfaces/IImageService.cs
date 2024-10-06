using Gestura.Models;

namespace Gestura.Interfaces
{
    public interface IImageService
    {
        Task<bool> DeleteImageAsync(ImageReference image);
        Task<ImageReference> ImportImageFromLocalAsync(string directoryName = null);
        Task<ImageReference> ImportImageFromUrlAsync(string imageUrl, string directoryName = null);
        Task<List<ImageReference>> GetAllImagesAsync();
        Task<List<ImageReference>> GetImagesByDirectoryIdAsync(int directoryId);
    }
}
