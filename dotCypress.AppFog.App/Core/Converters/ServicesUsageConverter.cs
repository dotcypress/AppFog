#region

using System;
using System.Globalization;
using System.Windows.Data;
using dotCypress.AppFog.Common.Models;
using dotCypress.AppFog.Localization;

#endregion

namespace dotCypress.AppFog.App.Core.Converters
{
    public class ServicesUsageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var info = value as Info;
            if (info != null)
            {
                if (targetType == typeof (string))
                {
                    return string.Format(AppResources.ServiceUsage, info.Usage.Services, info.Limits.Services);
                }
                return (double) info.Usage.Services/info.Limits.Services*100;
            }
            return targetType == typeof (string) ? (object) string.Format(AppResources.ServiceUsage, 0, 0): 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}