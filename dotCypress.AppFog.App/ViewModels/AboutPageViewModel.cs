#region

using Electrum;
using Electrum.IoC;
using Electrum.Logging;
using Electrum.MVVM;
using dotCypress.AppFog.App.Core.Commands;
using dotCypress.AppFog.Localization;

#endregion

namespace dotCypress.AppFog.App.ViewModels
{
    public class AboutPageViewModel : ViewModel
    {
        #region Protected members

        protected override void OnLoad()
        {
            Logger.Trace("Loading AboutPageViewModel ...");
        }

        #endregion

        #region Injection

        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public SendFeedbackCommand SendFeedbackCommand { get; set; }

        #endregion

        #region Properties

        public string AppVersion
        {
            get
            {
                var version = Extensions.GetAppVersion();
                return string.Format("{0} {1}.{2}", AppResources.Version, version.Major, version.Minor);
            }
        }

        #endregion
    }
}