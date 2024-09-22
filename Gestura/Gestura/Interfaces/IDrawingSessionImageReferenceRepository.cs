using Gestura.Models;

namespace Gestura.Interfaces
{
    public interface IDrawingSessionImageReferenceRepository
    {
        Task<int> AddRelationAsync(int drawingSessionId, int imageReferenceId);

        Task<int> DeleteRelationAsync(int drawingSessionId, int imageReferenceId);
        Task<int> DeleteRelationsBySessionIdAsync(int drawingSessionId);

        Task<List<ImageReference>> GetImagesForSessionAsync(int drawingSessionId);

        Task<List<DrawingSession>> GetSessionsForImageAsync(int imageReferenceId);
    }
}
