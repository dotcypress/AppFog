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
using dotCypress.AppFog.Common;
using dotCypress.AppFog.Common.Models;
using dotCypress.AppFog.Common.Network;
using dotCypress.AppFog.Localization;

#endregion

namespace dotCypress.AppFog.App.ViewModels
{
    public class AppInfoPageViewModel : ViewModel
    {
        #region Constants

        private const string GetStatsToken = "GetStats";
        private const string GetAppToken = "GeApp";
        private const string UpdateToken = "Update";

        #endregion

        #region Dependency registrations

        public static readonly DependencyProperty AppProperty =
            DependencyProperty.Register("App", typeof (AppInfo), typeof (AppInfoPageViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty ServicesProperty =
            DependencyProperty.Register("Services", typeof (List<Service>), typeof (AppInfoPageViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty InstancesProperty =
            DependencyProperty.Register("Instances", typeof (List<Instance>), typeof (AppInfoPageViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty RamProperty =
            DependencyProperty.Register("Ram", typeof (int), typeof (AppInfoPageViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty InstancesAmountProperty =
            DependencyProperty.Register("InstancesAmount", typeof (int), typeof (AppInfoPageViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty HasChangesProperty =
            DependencyProperty.Register("HasChanges", typeof (bool), typeof (AppInfoPageViewModel), new PropertyMetadata(false));

        #endregion

        #region Protected members

        protected override void OnLoad()
        {
            Logger.Trace("Loading AppInfoPageViewModel ...");
            if (Repository.Apps == null)
            {
                NavigationManager.GoBack();
                return;
            }
            App = Repository.Apps.FirstOrDefault(x => x.Name == AppName);
            if (App == null)
            {
                NavigationManager.GoBack();
                return;
            }
            RefreshInstances();
        }

        #endregion

        #region Injection

        [NavigationParam("name")]
        public string AppName { get; set; }

        [Inject]
        public AppState AppState { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public INavigationManager NavigationManager { get; set; }

        [Inject]
        public Repository Repository { get; set; }

        [Inject]
        public IApiClient ApiClient { get; set; }

        public ICommand ChangeRamCommand
        {
            get { return new RelayCommand<string>(ChangeRam); }
        }

        public ICommand ChangeInstancesCommand
        {
            get { return new RelayCommand<string>(ChangeInstances); }
        }

        public ICommand RefreshCommand
        {
            get { return new RelayCommand<object>(x => Refresh(), x => x == null || !((bool) x)); }
        }

        public ICommand StartStopCommand
        {
            get { return new RelayCommand<object>(x => StartStop(), x => x == null || !((bool) x)); }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand<object>(x =>
                                                    {
                                                        App.Instances = InstancesAmount;
                                                        App.Resources.Memory = Ram;
                                                        UpdateApp();
                                                    }, x => x == null || ((bool) x));
            }
        }

        #endregion

        #region Properties

        [Tombstoned]
        public AppInfo App
        {
            get { return (AppInfo) GetValue(AppProperty); }
            set { SetValue(AppProperty, value); }
        }

        [Tombstoned]
        public List<Service> Services
        {
            get { return (List<Service>) GetValue(ServicesProperty); }
            set { SetValue(ServicesProperty, value); }
        }

        [Tombstoned]
        public List<Instance> Instances
        {
            get { return (List<Instance>) GetValue(InstancesProperty); }
            set { SetValue(InstancesProperty, value); }
        }

        [Tombstoned]
        public int Ram
        {
            get { return (int) GetValue(RamProperty); }
            set { SetValue(RamProperty, value); }
        }

        [Tombstoned]
        public int InstancesAmount
        {
            get { return (int) GetValue(InstancesAmountProperty); }
            set { SetValue(InstancesAmountProperty, value); }
        }

        [Tombstoned]
        public bool HasChanges
        {
            get { return (bool) GetValue(HasChangesProperty); }
            set { SetValue(HasChangesProperty, value); }
        }

        #endregion

        #region Private members

        private void ChangeInstances(string op)
        {
            switch (op)
            {
                case "+":
                    InstancesAmount++;
                    break;
                case "-":
                    InstancesAmount--;
                    break;
            }
            if (InstancesAmount == 0)
            {
                InstancesAmount = 1;
            }
            HasChanges = true;
        }

        private void ChangeRam(string op)
        {
            switch (op)
            {
                case "+":
                    Ram = Ram + 64;
                    break;
                case "-":
                    Ram = Ram - 64;
                    break;
            }
            if (Ram < 128)
            {
                Ram = 128;
            }
            HasChanges = true;
        }

        private void Refresh()
        {
            AppState.ShowIndeterminate(AppResources.GetApp, GetAppToken);
            ApiClient.GetInfo((info, e) =>
                                  {
                                      Repository.Info = info;
                                      ApiClient.GetApp(AppName, (app, ex) =>
                                                                    {
                                                                        AppState.Hide(GetAppToken);
                                                                        if (ex != null || app == null)
                                                                        {
                                                                            MessageBox.Show(AppResources.Error, AppResources.AppTitle, MessageBoxButton.OK);
                                                                            return;
                                                                        }
                                                                        App = app;
                                                                        var existing = Repository.Apps.FirstOrDefault(x => x.Name == AppName);
                                                                        if (existing != null)
                                                                        {
                                                                            Repository.Apps.Remove(existing);
                                                                        }
                                                                        Repository.Apps.Add(app);
                                                                        RefreshInstances();
                                                                    });
                                  });
        }

        private void RefreshInstances()
        {
            if (Repository.Services != null && App.Services != null)
            {
                Services = Repository.Services.Where(x => App.Services.Contains(x.Name)).ToList();
            }
            InstancesAmount = App.Instances;
            Ram = App.Resources.Memory;
            AppState.ShowIndeterminate(AppResources.GetStats, GetStatsToken);
            ApiClient.GetAppStats(AppName, (instances, ex) =>
                                               {
                                                   AppState.Hide(GetStatsToken);
                                                   if (ex != null)
                                                   {
                                                       MessageBox.Show(AppResources.Error, AppResources.AppTitle, MessageBoxButton.OK);
                                                   }
                                                   else
                                                   {
                                                       Instances = instances.Values.ToList();
                                                   }
                                               });
        }

        private void StartStop()
        {
            App.State = App.State == "STARTED" ? "STOPPED" : "STARTED";
            UpdateApp();
        }

        private void UpdateApp()
        {
            HasChanges = false;
            AppState.ShowIndeterminate(AppResources.UpdateApp, UpdateToken);
            ApiClient.UpdateApp(App, (app, ex) =>
                                         {
                                             AppState.Hide(UpdateToken);
                                             if (ex != null)
                                             {
                                                 var errorMessage = ex.Message == "Forbidden" ? AppResources.NoMemory : AppResources.Error;
                                                 MessageBox.Show(errorMessage, AppResources.AppTitle, MessageBoxButton.OK);
                                             }
                                             Refresh();
                                         });
        }

        #endregion
    }
}