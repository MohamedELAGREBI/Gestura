using Gestura.Interfaces;
using Gestura.Models;
using Gestura.Services;
using SQLite;

namespace Gestura.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public ImageRepository()
        {
            _database = MauiProgram.Services.GetService<DatabaseService>().GetConnection();
        }

        public Task<int> DeleteImageAsync(ImageReference image)
        {
            return _database.DeleteAsync(image);
        }

        public Task<List<ImageReference>> GetAllImagesAsync()
        {
            return _database.Table<ImageReference>().ToListAsync();
        }

        public Task<ImageReference> GetImagesAsync(int id)
        {
            return _database.Table<ImageReference>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveImageAsync(ImageReference image)
        {
            if (image.Id == 0)
            {
                image.CreatedAt = DateTime.Now;
                image.UpdatedAt = DateTime.Now;
                return _database.InsertAsync(image);
            }
            else
            {
                image.UpdatedAt = DateTime.Now;
                return _database.UpdateAsync(image);
            }
        }
    }
}
