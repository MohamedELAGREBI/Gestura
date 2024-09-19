using Gestura.Models;

namespace Gestura.Interfaces
{
    public interface IImageRepository
    {
        Task<List<ImageReference>> GetAllImagesAsync();

        Task<ImageReference> GetImagesAsync(int id);

        Task<int> SaveImageAsync(ImageReference image);

        Task<int> DeleteImageAsync(ImageReference image);
    }
}
