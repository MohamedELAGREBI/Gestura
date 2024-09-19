using System.Globalization;

namespace Gestura.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }

            return false; // Par défaut, retourne 'false' si la valeur n'est pas un booléen
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }

            return false; // Par défaut, retourne 'false' si la valeur n'est pas un booléen
        }
    }
}
