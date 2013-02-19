#region

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using dotCypress.AppFog.Common.Models;

#endregion

namespace dotCypress.AppFog.App.Core.Converters
{
    public class ApplicationStateConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var app = value as AppInfo;
            if (app != null)
            {
                var isStarted = app.State.ToLower() == "started";
                if (targetType == typeof (Uri))
                {
                    return isStarted ? new Uri("/Assets/Images/appbar.stop.rest.png", UriKind.RelativeOrAbsolute) : new Uri("/Assets/Images/appbar.play.rest.png", UriKind.RelativeOrAbsolute);
                }
                if (targetType == typeof (Brush))
                {
                    return isStarted ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                }
                if (targetType == typeof (string))
                {
                    return isStarted ? "running" : "stopped";
                }
                return isStarted;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}