using Gestura.Interfaces;
using Gestura.Models;
using Gestura.Services;
using SQLite;

namespace Gestura.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly IDrawingSessionImageReferenceRepository _relationRepository;

        public ImageRepository()
        {
            _database = MauiProgram.Services.GetService<DatabaseService>().GetConnection();
            _relationRepository = MauiProgram.Services.GetService<IDrawingSessionImageReferenceRepository>();
        }

        public async Task<int> DeleteImageAsync(ImageReference image)
        {
            var sessionsWithThisImage = await _relationRepository.GetSessionsForImageAsync(image.Id);
            foreach (var session in sessionsWithThisImage)
            {
                await _relationRepository.DeleteRelationAsync(session.Id, image.Id);
            }
            return await _database.DeleteAsync(image);
        }

        public Task<List<ImageReference>> GetAllImagesAsync()
        {
            return _database.Table<ImageReference>().ToListAsync();
        }

        public Task<ImageReference> GetImagesAsync(int id)
        {
            return _database.Table<ImageReference>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<ImageReference>> GetImagesByDirectoryIdAsync(int directoryId)
        {
            return _database.Table<ImageReference>().Where(i => i.DirectoryId == directoryId).ToListAsync();
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
