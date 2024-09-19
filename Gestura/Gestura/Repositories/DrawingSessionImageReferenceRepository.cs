using Gestura.Interfaces;
using Gestura.Models;
using Gestura.Services;
using SQLite;

namespace Gestura.Repositories
{
    public class DrawingSessionImageReferenceRepository : IDrawingSessionImageReferenceRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public DrawingSessionImageReferenceRepository()
        {
            _database = MauiProgram.Services.GetService<DatabaseService>().GetConnection();
        }

        public async Task<int> AddRelationAsync(int drawingSessionId, int imageReferenceId)
        {
            var relation = new DrawingSessionImageReference
            {
                DrawingSessionId = drawingSessionId,
                ImageReferenceId = imageReferenceId
            };

            return await _database.InsertAsync(relation);
        }

        public async Task<int> DeleteRelationAsync(int drawingSessionId, int imageReferenceId)
        {
            return await _database.ExecuteAsync(
                "DELETE FROM DRAWING_SESSION_IMAGE_REFERENCE WHERE DrawingSessionId = ? AND ImageReferenceId = ?",
                drawingSessionId, imageReferenceId);
        }

        public async Task<List<ImageReference>> GetImagesForSessionAsync(int drawingSessionId)
        {
            var query = @"SELECT IMAGE_REFERENCES.* FROM IMAGE_REFERENCES
JOIN DRAWING_SESSION_IMAGE_REFERENCE
ON IMAGE_REFERENCES.Id = DRAWING_SESSION_IMAGE_REFERENCE.ImageReferenceId
WHERE DRAWING_SESSION_IMAGE_REFERENCE.DrawingSessionId = ?";

            return await _database.QueryAsync<ImageReference>(query, drawingSessionId);
        }

        public async Task<List<DrawingSession>> GetSessionsForImageAsync(int imageReferenceId)
        {
            var query = @"SELECT DRAWING_SESSIONS.* FROM DRAWING_SESSIONS
JOIN DRAWING_SESSION_IMAGE_REFERENCE
ON DRAWING_SESSIONS.Id = DRAWING_SESSION_IMAGE_REFERENCE.DrawingSessionId
WHERE DRAWING_SESSION_IMAGE_REFERENCE.ImageReferenceId = ?";

            return await _database.QueryAsync<DrawingSession>(query, imageReferenceId);
        }
    }
}
