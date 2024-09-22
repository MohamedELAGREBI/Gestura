using Gestura.Interfaces;
using Gestura.Models;
using Gestura.Services;
using SQLite;

namespace Gestura.Repositories
{
    public class DrawingSessionRepository : IDrawingSessionRepository
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly IDrawingSessionImageReferenceRepository _relationRepository;

        public DrawingSessionRepository()
        {
            _database = MauiProgram.Services.GetService<DatabaseService>().GetConnection();
            _relationRepository = MauiProgram.Services.GetService<IDrawingSessionImageReferenceRepository>();
        }

        public async Task<int> DeleteSessionAsync(DrawingSession drawingSession)
        {
            return await _database.DeleteAsync(drawingSession);
        }

        public async Task<List<DrawingSession>> GetAllDrawingSessionsAsync()
        {
            var sessions = await _database.Table<DrawingSession>().ToListAsync();
            foreach (var session in sessions)
            {
                var sessionImages = await _relationRepository.GetImagesForSessionAsync(session.Id);
                session.SelectedImages = sessionImages;
            }

            return sessions;
        }

        public async Task<DrawingSession> GetDrawingSessionByIdAsync(int id)
        {
            var session = await _database.Table<DrawingSession>().Where(s => s.Id == id).FirstOrDefaultAsync();
            session.SelectedImages = await _relationRepository.GetImagesForSessionAsync(id);
            return session;
        }

        public async Task<int> SaveSessionAsync(DrawingSession drawingSession)
        {
            if (drawingSession.Id > 0)
            {
                drawingSession.LastUpdateAt = DateTime.Now;
                return await _database.UpdateAsync(drawingSession);
            }
            else
            {
                drawingSession.CreatedAt = DateTime.Now;
                drawingSession.LastUpdateAt = drawingSession.CreatedAt;
                return await _database.InsertAsync(drawingSession);
            }
        }
    }
}
