using Gestura.Commons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestura.Converters
{
    public class ImportMethodToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ImportMethodEnum method)
                return "";

            return method switch
            {
                ImportMethodEnum.LocalStorage => "Depuis la mémoire",     // futur: AppResources.LocalLabel
                ImportMethodEnum.UrlWeb => "Depuis une URL",          // futur: AppResources.UrlLabel
                _ => method.ToString()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Non nécessaire pour un Picker en lecture seule
            throw new NotImplementedException();
        }
    }
}
