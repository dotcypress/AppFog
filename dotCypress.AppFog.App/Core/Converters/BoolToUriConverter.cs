#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace dotCypress.AppFog.App.Core.Converters
{
    public class BoolToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool && parameter != null)
            {
                var boolValue = (bool) value;
                var uries = parameter.ToString().Split('|');
                return new Uri(uries[boolValue ? 0 : 1], UriKind.RelativeOrAbsolute);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}