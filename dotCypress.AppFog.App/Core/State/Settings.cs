#region

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Electrum;
using Electrum.Settings;
using dotCypress.AppFog.Common.Settings;

#endregion

namespace dotCypress.AppFog.App.Core.State
{
    public class Settings : ISettings
    {
        private readonly ISettingsManager _settingsManager;

        public Settings(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        #region Properties

        public string Username
        {
            get { return _settingsManager.GetValue(() => Username, ""); }
            set
            {
                _settingsManager.SetValue(() => Username, value);
                _settingsManager.Save();
                OnPropertyChanged(() => Username);
            }
        }

        public string Password
        {
            get { return _settingsManager.GetValue(() => Password, ""); }
            set
            {
                _settingsManager.SetValue(() => Password, value);
                _settingsManager.Save();
                OnPropertyChanged(() => Password);
            }
        }

        #endregion

        #region Private members

        private void OnPropertyChanged(Expression<Func<object>> property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property.GetPropertyName()));
            }
        }

        #endregion

        #region ISettings Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}