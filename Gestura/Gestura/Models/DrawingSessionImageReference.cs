using SQLite;

namespace Gestura.Models
{
    [Table("DRAWING_SESSION_IMAGE_REFERENCE")]
    public class DrawingSessionImageReference
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int DrawingSessionId { get; set; }
        public int ImageReferenceId { get; set; }
    }
}
