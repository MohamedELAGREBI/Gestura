using SQLite;

namespace Gestura.Models
{
    [Table("DIRECTORIES")]
    public class Directory
    {
        [PrimaryKey, AutoIncrement]
        public virtual int Id { get; set; }

        [Unique]
        public virtual string Name { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdateAt { get; set; }

        public Directory()
        {
            CreatedAt = DateTime.Now;
            UpdateAt = CreatedAt;
        }
    }
}
