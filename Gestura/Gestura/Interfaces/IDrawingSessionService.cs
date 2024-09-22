using Gestura.Models;

namespace Gestura.Interfaces
{
    public interface IDrawingSessionService
    {
        Task<bool> AddOrUpdateSessionAsync(DrawingSession drawingSession);
        Task<bool> AddImageToSessionAsync(int drawingSessionId, int imageReferenceId);
        Task<bool> DeleteSessionAsync(DrawingSession drawingSession);
        Task<List<ImageReference>> GetImagesForSessionAsync(int drawingSessionId);
        Task<List<DrawingSession>> GetSessionsAsync();
    }
}
