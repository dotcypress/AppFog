#region

using System;
using System.Collections.Generic;
using dotCypress.AppFog.Common.Models;

#endregion

namespace dotCypress.AppFog.Common.Network
{
    public interface IApiClient
    {
        void Login(string email, string password, Action<bool, Exception> callback);
        void GetInfo(Action<Info, Exception> callback);
        void GetServices(Action<List<Service>, Exception> callback);
        void GetApps(Action<List<AppInfo>, Exception> callback);
        void GetAppStats(string appName, Action<Dictionary<string, Instance>, Exception> callback);
        void Logoff();
        void GetApp(string appName, Action<AppInfo, Exception> callback);
        void UpdateApp(AppInfo app,Action<bool, Exception> callback);
    }
}