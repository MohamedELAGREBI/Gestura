using Gestura.Models;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Gestura.Converters
{
    public class ImageSelectionConverter : IValueConverter
    {
        // Paramètres : l'image actuelle et la collection des images sélectionnées
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentImage = value as ImageReference;
            var selectedImages = parameter as ObservableCollection<ImageReference>;

            if (currentImage != null && selectedImages != null)
            {
                // Si l'image actuelle fait partie des images sélectionnées, elle est sélectionnée
                return selectedImages.Contains(currentImage);
            }

            return false;  // Si elle n'est pas dans la collection, elle est non sélectionnée
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
