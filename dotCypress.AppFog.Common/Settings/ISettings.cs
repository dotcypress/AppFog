#region

using System.ComponentModel;

#endregion

namespace dotCypress.AppFog.Common.Settings
{
    public interface ISettings : INotifyPropertyChanged
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}