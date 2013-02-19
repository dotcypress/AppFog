#region

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Electrum.Commands;
using Electrum.IoC;
using Electrum.Logging;
using Electrum.MVVM;
using Electrum.Navigation;
using Electrum.State;
using dotCypress.AppFog.App.Core.Commands;
using dotCypress.AppFog.App.Core.Navigation;
using dotCypress.AppFog.Common;
using dotCypress.AppFog.Common.Models;
using dotCypress.AppFog.Common.Network;
using dotCypress.AppFog.Common.Settings;
using dotCypress.AppFog.Localization;

#endregion

namespace dotCypress.AppFog.App.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        #region Constants

        private const string LoginToken = "Login";
        private const string GetAppsToken = "GetApps";

        #endregion

        #region Dependency registrations

        public static readonly DependencyProperty LoggedInProperty =
            DependencyProperty.Register("LoggedIn", typeof (bool), typeof (MainPageViewModel), new PropertyMetadata(true));

        public static readonly DependencyProperty AppsProperty =
            DependencyProperty.Register("Apps", typeof (List<AppInfo>), typeof (MainPageViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty InfoProperty =
            DependencyProperty.Register("Info", typeof (Info), typeof (MainPageViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty ServicesProperty =
            DependencyProperty.Register("Services", typeof (List<Service>), typeof (MainPageViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof (string), typeof (MainPageViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof (string), typeof (MainPageViewModel), new PropertyMetadata(null));

        #endregion

        #region Protected members

        protected override void OnCreate()
        {
            Logger.Trace("Loading MainPageViewModel ...");
            Username = Settings.Username;
            Password = Settings.Password;
            if (string.IsNullOrWhiteSpace(Settings.Password))
            {
                LoggedIn = false;
            }
            else
            {
                Login();
            }
        }

        protected override void OnLoad()
        {
            if (Repository.Apps != null)
            {
                Apps = Repository.Apps.OrderBy(x => x.Name).ToList();
            }
            Info = Repository.Info;
        }

        #endregion

        #region Injection

        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public INavigationManager NavigationManager { get; set; }

        [Inject]
        public ISettings Settings { get; set; }

        [Inject]
        public Repository Repository { get; set; }

        [Inject]
        public IApiClient ApiClient { get; set; }

        [Inject]
        public OpenAboutCommand OpenAboutCommand { get; set; }

        public ICommand LoginCommand
        {
            get { return new RelayCommand<string>(x => Login(), s => !string.IsNullOrEmpty(s)); }
        }

        public ICommand LogoffCommand
        {
            get { return new RelayCommand<string>(x => Logoff()); }
        }

        public ICommand RefreshCommand
        {
            get { return new RelayCommand<string>(x => Refresh()); }
        }

        public ICommand OpenApplicationCommand
        {
            get { return new RelayCommand<AppInfo>(x => NavigationManager.GoToApplicationPage(x.Name)); }
        }

        #endregion

        #region Properties

        [Tombstoned]
        public Info Info
        {
            get { return (Info) GetValue(InfoProperty); }
            set { SetValue(InfoProperty, value); }
        }

        [Tombstoned]
        public List<AppInfo> Apps
        {
            get { return (List<AppInfo>) GetValue(AppsProperty); }
            set { SetValue(AppsProperty, value); }
        }

        [Tombstoned]
        public List<Service> Services
        {
            get { return (List<Service>) GetValue(ServicesProperty); }
            set { SetValue(ServicesProperty, value); }
        }

        public string Username
        {
            get { return (string) GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public string Password
        {
            get { return (string) GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        [Tombstoned]
        public bool LoggedIn
        {
            get { return (bool) GetValue(LoggedInProperty); }
            set { SetValue(LoggedInProperty, value); }
        }

        #endregion

        #region Private members

        private void Login()
        {
            AppState.ShowIndeterminate(AppResources.Loginin, LoginToken);
            ApiClient.Login(Username, Password, (res, ex) =>
                                                    {
                                                        AppState.Hide(LoginToken);
                                                        if (ex != null)
                                                        {
                                                            MessageBox.Show(AppResources.Error, AppResources.AppTitle, MessageBoxButton.OK);
                                                        }
                                                        else if (res)
                                                        {
                                                            Settings.Username = Username;
                                                            Settings.Password = Password;
                                                            Refresh();
                                                            LoggedIn = true;
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show(AppResources.CantLogin, AppResources.AppTitle, MessageBoxButton.OK);
                                                            Logoff();
                                                        }
                                                    });
        }

        private void Logoff()
        {
            ApiClient.Logoff();
            Settings.Password = null;
            Password = null;
            LoggedIn = false;
        }

        private void Refresh()
        {
            ApiClient.GetInfo((info, ex) =>
                                  {
                                      Repository.Info = info;
                                      if (ex != null)
                                      {
                                          return;
                                      }
                                      Info = info;
                                  });
            ApiClient.GetServices((services, ex) =>
                                      {
                                          Repository.Services = services;
                                          if (ex != null)
                                          {
                                              return;
                                          }
                                          Services = services.OrderBy(x => x.Name).ToList();
                                      });
            AppState.ShowIndeterminate(AppResources.GetApps, GetAppsToken);
            ApiClient.GetApps((apps, ex) =>
                                  {
                                      AppState.Hide(GetAppsToken);
                                      Repository.Apps = apps;
                                      if (ex != null)
                                      {
                                          return;
                                      }
                                      Apps = apps.OrderBy(x => x.Name).ToList();
                                  });
        }

        #endregion
    }
}