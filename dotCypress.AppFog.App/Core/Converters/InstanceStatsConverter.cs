#region

using System;
using System.Globalization;
using System.Windows.Data;
using dotCypress.AppFog.Common.Models;

#endregion

namespace dotCypress.AppFog.App.Core.Converters
{
    public class InstanceStatsConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var instance = value as Instance;
            if (instance != null && instance.Stats != null && instance.Stats.Usage != null)
            {
                switch (parameter.ToString())
                {
                    case "CPU":
                        return string.Format("CPU: {0}%", instance.Stats.Usage.Cpu);
                    case "MEMORY":
                        var memUsage = Math.Round(((double) instance.Stats.Usage.Mem*1024)/instance.Stats.MemQuota*100, 2);
                        return string.Format("RAM: {0}%", memUsage);
                    case "DISK":
                        var diskUsage = Math.Round((double) instance.Stats.Usage.Disk/instance.Stats.DiskQuota*100, 2);
                        return string.Format("DISK: {0}%", diskUsage);
                }
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