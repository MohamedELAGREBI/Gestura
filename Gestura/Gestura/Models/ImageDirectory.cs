using SQLite;

namespace Gestura.Models
{
    [Table("IMAGE_DIRECTORIES")]
    public class ImageDirectory
    {
        [PrimaryKey, AutoIncrement]
        public virtual int Id { get; set; }

        [Unique]
        public virtual string Name { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdateAt { get; set; }

        public virtual string Path { get; set; }

        [Ignore]
        public ICollection<ImageReference> ImageReferences { get; set; }

        public ImageDirectory()
        {
            CreatedAt = DateTime.Now;
            UpdateAt = CreatedAt;
        }
    }
}
