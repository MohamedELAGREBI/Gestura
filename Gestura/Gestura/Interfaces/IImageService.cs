using Gestura.Models;

namespace Gestura.Interfaces
{
    public interface IImageService
    {
        Task<bool> DeleteImageAsync(ImageReference image);
        Task<ImageReference> ImportImageFromLocalAsync();
        Task<ImageReference> ImportImageFromUrlAsync(string imageUrl);
        Task<List<ImageReference>> GetAllImagesAsync();
    }
}
