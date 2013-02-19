#region

using System;
using System.Globalization;

#endregion

namespace dotCypress.AppFog.App.Core
{
    public static class Helpers
    {
        public static string ToReadableSize(this double size)
        {
            string[] suf = {"B", "KB", "MB", "GB", "TB", "PB"};
            var place = Convert.ToInt32(Math.Floor(Math.Log(size, 1024)));
            var num = Math.Round(size/Math.Pow(1024, place), 1);
            return num.ToString(CultureInfo.InvariantCulture) + suf[place];
        }
    }
}