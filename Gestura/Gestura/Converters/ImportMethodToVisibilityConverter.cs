using Gestura.Commons;
using System.Globalization;

namespace Gestura.Converters
{
    public class ImportMethodToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ImportMethodEnum method && parameter is string targetMethod)
            {
                return method.ToString() == targetMethod;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
