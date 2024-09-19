using System.Globalization;

namespace Gestura.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                if (timeSpan.Hours > 0)
                {
                    return $"{timeSpan.Hours}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
                }
                else if (timeSpan.Minutes > 0)
                {
                    return $"{timeSpan.Minutes}:{timeSpan.Seconds:D2}";
                }
                else
                {
                    return $"{timeSpan.Seconds}";
                }
            }
            return "0";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
