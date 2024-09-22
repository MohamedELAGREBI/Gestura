using Gestura.Interfaces;
using Gestura.Models;

namespace Gestura.Services
{
    public class DrawingSessionService : IDrawingSessionService
    {
        private readonly IDrawingSessionRepository _drawingSessionRepository;
        private readonly IDrawingSessionImageReferenceRepository _relationImageRepository;

        public DrawingSessionService(IDrawingSessionRepository drawingSessionRepository, IDrawingSessionImageReferenceRepository relationRepository)
        {
            _drawingSessionRepository = drawingSessionRepository ?? throw new ArgumentNullException(nameof(drawingSessionRepository));
            _relationImageRepository = relationRepository ?? throw new ArgumentNullException(nameof(relationRepository));
        }

        public async Task<List<DrawingSession>> GetSessionsAsync()
        {
            return await _drawingSessionRepository.GetAllDrawingSessionsAsync();
        }

        public async Task<bool> AddOrUpdateSessionAsync(DrawingSession drawingSession)
        {
            var isSessionSaved = await _drawingSessionRepository.SaveSessionAsync(drawingSession) > 0;

            if (!isSessionSaved)
            {
                return false;
            }

            if (drawingSession.Id > 0)
            {
                await _relationImageRepository.DeleteRelationsBySessionIdAsync(drawingSession.Id);
            }

            foreach (var image in drawingSession.SelectedImages)
            {
                await _relationImageRepository.AddRelationAsync(drawingSession.Id, image.Id);
            }

            return true;
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
