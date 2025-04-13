using Gestura.Models;

namespace Gestura.Interfaces
{
    public interface IImageDirectoryRepository
    {
        Task<List<ImageDirectory>> GetAllAsync();
        Task<ImageDirectory> InsertAsync(ImageDirectory directory);
        Task<int> UpdateAsync(ImageDirectory directory);
        Task<int> DeleteAsync(ImageDirectory directory);
        public Task<ImageDirectory> GetDirectoryByIdAsync(int id);
        public Task<ImageDirectory> GetDirectoryByNameAsync(string name);
    }
}
