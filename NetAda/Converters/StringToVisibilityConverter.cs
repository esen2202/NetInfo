using System;
using System.Globalization;
using System.Windows.Data;

namespace NetAda.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value != null ? (string) value : "";
            return str != "" ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
