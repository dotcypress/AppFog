#region

using System;
using System.Globalization;
using System.Windows.Data;
using dotCypress.AppFog.Localization;

#endregion

namespace dotCypress.AppFog.App.Core.Converters
{
    public class OffOnConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            if (targetType != typeof (object))
            {
                throw new ArgumentException("targetType");
            }
            if (value is bool? || value == null)
            {
                return (bool?) value == true ? AppResources.On : AppResources.Off;
            }
            throw new ArgumentException("value");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}