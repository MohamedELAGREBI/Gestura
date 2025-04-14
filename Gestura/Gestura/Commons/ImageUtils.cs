namespace Gestura.Commons
{
    public static class ImageUtils
    {
        public const int IMAGE_MAX_WIDTH = 1920;
        public const int IMAGE_MAX_HEIGHT = 1080;
        public const int IMAGE_MAX_RESOLUTION = IMAGE_MAX_HEIGHT * IMAGE_MAX_WIDTH; // 2 073 600 pixels

        public static bool ShouldResize(int width, int height)
        {
            return (width * height) > IMAGE_MAX_RESOLUTION;
        }

        public static (int newWidth, int newHeight) GetResizedDimensions(int originalWidth, int originalHeight)
        {
            // Vérification des dimensions d'origine
            if (originalWidth <= 0 || originalHeight <= 0)
            {
                throw new ArgumentException("Les dimensions d'origine doivent être supérieures à zéro.");
            }

            // Vérification des dimensions maximales
            if (IMAGE_MAX_WIDTH <= 0 || IMAGE_MAX_HEIGHT <= 0)
            {
                throw new InvalidOperationException("Les dimensions maximales doivent être définies et supérieures à zéro.");
            }

            // Si l'image est déjà dans les limites, aucune modification n'est nécessaire
            if (originalWidth <= IMAGE_MAX_WIDTH && originalHeight <= IMAGE_MAX_HEIGHT)
            {
                return (originalWidth, originalHeight);
            }

            // Calculer le facteur d'échelle
            var widthRatio = (double)IMAGE_MAX_WIDTH / originalWidth;
            var heightRatio = (double)IMAGE_MAX_HEIGHT / originalHeight;
            var scaleFactor = Math.Min(widthRatio, heightRatio);

            // Calculer les nouvelles dimensions en conservant les proportions
            var newWidth = Math.Max(1, (int)Math.Round(originalWidth * scaleFactor));
            var newHeight = Math.Max(1, (int)Math.Round(originalHeight * scaleFactor));

            return (newWidth, newHeight);
        }
    }
}
