using System.Globalization;

namespace Gestura.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = (bool)value;
            return isSelected ? Colors.Blue : Colors.Gray; // Bleu si sélectionné, Gris sinon
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
