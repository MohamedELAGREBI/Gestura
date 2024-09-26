using SQLite;

namespace Gestura.Models
{
    [Table("DRAWING_SESSIONS")]
    public class DrawingSession
    {
        [PrimaryKey, AutoIncrement]
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }
        public virtual TimeSpan PoseDuration { get; set; }
        public virtual bool IsLimitless { get; set; }

        [Ignore]
        public List<ImageReference> SelectedImages { get; set; } = new List<ImageReference>();

        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime LastUpdateAt { get; set; }
        public virtual bool IsCompleted { get; set; }

        public DrawingSession()
        {
            InitializeDefaults();
        }

        public DrawingSession(string title, TimeSpan poseDuration, bool isLimitless)
        {
            Title = title;
            PoseDuration = poseDuration;
            IsLimitless = isLimitless;
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            CreatedAt = DateTime.Now;
            LastUpdateAt = CreatedAt;
            PoseDuration = new TimeSpan(0, 10, 0);

            if (string.IsNullOrWhiteSpace(Title))
            {
                Title = "Session du " + CreatedAt.ToString("f"); // => Session du lundi 15 juin 2009 13:45
            }

            IsCompleted = false;
        }

        public bool CanStart()
        {
            return SelectedImages != null && SelectedImages.Count > 0;
        }
    }
}
