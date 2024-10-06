using SQLite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Gestura.Models
{
    [SQLite.Table("IMAGE_REFERENCES")]
    public class ImageReference
    {
        [PrimaryKey, AutoIncrement]
        public virtual int Id { get; set; }

        public virtual string FileName { get; set; }
        public virtual string Metadata { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }

        private string _filePath;
        public virtual string FilePath
        {
            get => _filePath;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le chemin du fichier est invalide : " + value);
                }

                _filePath = value;
            }
        }

        private string _directoryPath;
        public virtual string DirectoryPath
        {
            get => _directoryPath;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Le chemin du répertoire est invalide : " + value);
                }

                _directoryPath = value;
            }
        }

        [ForeignKey(nameof(Directory))]
        public virtual int DirectoryId { get; set; }

        [Ignore]
        public List<string> Tags { get; set; } = new List<string>();
        public string TagsJsonified
        {
            get => JsonSerializer.Serialize(Tags);
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("La liste des tags est nulle ou vide.");
                }

                Tags = JsonSerializer.Deserialize<List<string>>(value);
            }
        }

        // TODO : Voir s'il ne faut pas ajouter des validateurs pour FileName (ex: extension fichier, ...), ou pour Tags (caractères interdits, taille totale ...)

        public ImageReference()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = CreatedAt;

            FileName = Guid.NewGuid().ToString().ToUpperInvariant() + ".jpg";
        }
    }
}
