using Gestura.Interfaces;
using Gestura.Models;
using Gestura.Services;
using SQLite;

namespace Gestura.Repositories
{
    public class DrawingSessionRepository : IDrawingSessionRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public DrawingSessionRepository()
        {
            _database = MauiProgram.Services.GetService<DatabaseService>().GetConnection();
        }

        public async Task<int> DeleteSessionAsync(DrawingSession drawingSession)
        {
            return await _database.DeleteAsync(drawingSession);
        }

        public async Task<List<DrawingSession>> GetAllDrawingSessionsAsync()
        {
            return await _database.Table<DrawingSession>().ToListAsync();
        }

        public async Task<DrawingSession> GetDrawingSessionByIdAsync(int id)
        {
            return await _database.Table<DrawingSession>().Where(s => s.Id == id).FirstOrDefaultAsync();
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
