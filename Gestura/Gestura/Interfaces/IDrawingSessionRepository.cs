using Gestura.Models;

namespace Gestura.Interfaces
{
    public interface IDrawingSessionRepository
    {
        Task<List<DrawingSession>> GetAllDrawingSessionsAsync();

        Task<DrawingSession> GetDrawingSessionByIdAsync(int id);

        Task<int> SaveSessionAsync(DrawingSession drawingSession);

        Task<int> DeleteSessionAsync(DrawingSession drawingSession);
    }
}
