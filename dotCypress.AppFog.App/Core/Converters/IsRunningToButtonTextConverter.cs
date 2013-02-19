#region

using System;
using System.Globalization;
using System.Windows.Data;
using dotCypress.AppFog.Common.Models;
using dotCypress.AppFog.Localization;

#endregion

namespace dotCypress.AppFog.App.Core.Converters
{
    public class IsRunningToButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var app = value as AppInfo;
            if (app != null)
            {
                var isStarted = app.State.ToLower() == "started";
                return isStarted ? AppResources.Stop : AppResources.Start;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}