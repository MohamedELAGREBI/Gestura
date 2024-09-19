using Gestura.Interfaces;
using Gestura.Models;

namespace Gestura.Services
{
    public class DrawingSessionService
    {
        private readonly IDrawingSessionRepository _drawingSessionRepository;
        private readonly IDrawingSessionImageReferenceRepository _relationImageRepository;

        public DrawingSessionService()
        {
            _drawingSessionRepository = MauiProgram.Services.GetService<IDrawingSessionRepository>();
            _relationImageRepository = MauiProgram.Services.GetService<IDrawingSessionImageReferenceRepository>();
        }

        public async Task<List<DrawingSession>> GetSessionsAsync()
        {
            return await _drawingSessionRepository.GetAllDrawingSessionsAsync();
        }

        public async Task<bool> AddSessionAsync(DrawingSession drawingSession)
        {
            return await _drawingSessionRepository.SaveSessionAsync(drawingSession) > 0;
        }

        public async Task<bool> UpdateSessionAsync(DrawingSession drawingSession)
        {
            return await _drawingSessionRepository.SaveSessionAsync(drawingSession) > 0;
        }

        public async Task<bool> DeleteSessionAsync(DrawingSession drawingSession)
        {
            return await _drawingSessionRepository.DeleteSessionAsync(drawingSession) > 0;
        }

        public async Task<bool> AddImageToSessionAsync(int drawingSessionId, int imageReferenceId)
        {
            return await _relationImageRepository.AddRelationAsync(drawingSessionId, imageReferenceId) > 0;
        }

        public async Task<List<ImageReference>> GetImagesForSessionAsync(int drawingSessionId)
        {
            return await _relationImageRepository.GetImagesForSessionAsync(drawingSessionId);
        }
    }
}
