using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace NetAda.Converters
{
    public class MacAddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var strObj = value != null? (string)value:"";

            var regex = "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})";
            var replace = "$1:$2:$3:$4:$5:$6";
            var newformat = Regex.Replace(strObj, regex, replace);

            return newformat;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
