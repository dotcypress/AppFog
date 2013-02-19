#region

using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Windows;
using System.Windows.Media;
using Electrum;
using Electrum.Controls;
using Electrum.Logging;
using Microsoft.Phone.Controls;
using dotCypress.AppFog.App.Core.Commands;
using dotCypress.AppFog.App.Core.State;
using dotCypress.AppFog.App.ViewModels;
using dotCypress.AppFog.App.Views;
using dotCypress.AppFog.Common;
using dotCypress.AppFog.Common.Network;
using dotCypress.AppFog.Common.Settings;

#endregion

namespace dotCypress.AppFog.App.Core
{
    public class AppBootstrapper : Bootstrapper
    {
        #region Fields

        private ILogger _logger;

        #endregion

        #region Protected members

        protected override void Init()
        {
#if DEBUG
            MetroGridHelper.IsVisible = true;
#endif
            InitLogger();
            InitContainer();
        }

        protected override PhoneApplicationFrame CreateRootFrame()
        {
            var animatedFrame = new AnimatedFrame {Background = new SolidColorBrush(Color.FromArgb(0xff, 0xe9, 0xf1, 0xf4))};
            return animatedFrame;
        }

        protected override void OnApplicationUnhandledException(ApplicationUnhandledExceptionEventArgs exceptionEventArgs)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        #endregion

        #region Private members

        private void InitLogger()
        {
#if DEBUG
            Container.RegisterInstance<ILogger, DebugLogger>();
#endif
            _logger = Container.Resolve<ILogger>();
        }

        private void InitContainer()
        {
            _logger.Trace("Container initalization started...");

            Container.DefineContructorParamRule<SendFeedbackCommand>("supportEmail", "dotcypress@gmail.com");
            Container.DefineContructorParamRule<ApiClient>("serviceUri", "https://api.appfog.com");

            Container.RegisterInstance((AppState) Application.Current.Resources["AppState"]);
            Container.RegisterInstance(new DispatcherScheduler(Deployment.Current.Dispatcher));
            Container.RegisterInstance<IScheduler>(ThreadPoolScheduler.Instance);
            Container.RegisterInstance<ISettings, Settings>();
            Container.RegisterInstance<IApiClient, ApiClient>();
            Container.RegisterInstance<Repository>();
            Container.RegisterInstance<AppInfoPageViewModel>();

            _logger.Trace("Registering view models");

            ViewModelMap.Register<MainPage, MainPageViewModel>();
            ViewModelMap.Register<AppInfoPage, AppInfoPageViewModel>();
            ViewModelMap.Register<AboutPage, AboutPageViewModel>();

            _logger.Trace("Container initalization complete");
        }

        #endregion
    }
}