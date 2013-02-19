#region

using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using dotCypress.AppFog.Common.Models;
using dotCypress.AppFog.Common.Network.Models;

#endregion

namespace dotCypress.AppFog.Common.Network
{
    public class ApiClient : IApiClient
    {
        public ApiClient(string serviceUri)
        {
            BaseUrl = serviceUri;
        }

        public string BaseUrl { get; set; }

        public IAuthenticator Authenticator { get; set; }

        public void Login(string email, string password, Action<bool, Exception> callback)
        {
            var request = new RestRequest
                              {
                                  Resource = string.Format("/users/{0}/tokens", email),
                                  Method = Method.POST,
                                  RequestFormat = DataFormat.Json
                              };
            request.AddBody(new LoginModel(password));
            Execute(request, (AuthModel auth, Exception e) =>
                                 {
                                     if (auth != null && auth.Token != null)
                                     {
                                         Authenticator = new HeaderAuthenticator(auth.Token);
                                         callback(true, null);
                                     }
                                     else
                                     {
                                         callback(false, e);
                                     }
                                 });
        }

        public void GetInfo(Action<Info, Exception> callback)
        {
            var request = new RestRequest
                              {
                                  Resource = "info",
                                  Method = Method.GET,
                                  RequestFormat = DataFormat.Json
                              };
            Execute(request, callback);
        }

        public void UpdateApp(AppInfo app, Action<bool, Exception> callback)
        {
            var request = new RestRequest
                              {
                                  Resource = string.Format("/apps/{0}", app.Name),
                                  Method = Method.PUT,
                                  RequestFormat = DataFormat.Json, JsonSerializer = new JsonSerializer()
                              };
            request.AddBody(app);
            Execute(request, callback);
        }

        public void GetServices(Action<List<Service>, Exception> callback)
        {
            var request = new RestRequest
                              {
                                  Resource = "services",
                                  Method = Method.GET,
                                  RequestFormat = DataFormat.Json
                              };
            Execute(request, callback);
        }

        public void GetApps(Action<List<AppInfo>, Exception> callback)
        {
            var request = new RestRequest
                              {
                                  Resource = "/apps",
                                  Method = Method.GET,
                                  RequestFormat = DataFormat.Json
                              };
            Execute(request, callback);
        }

        public void GetApp(string appName, Action<AppInfo, Exception> callback)
        {
            var request = new RestRequest
                              {
                                  Resource = string.Format("/apps/{0}", appName),
                                  Method = Method.GET,
                                  RequestFormat = DataFormat.Json
                              };
            Execute(request, callback);
        }

        public void GetAppStats(string appName, Action<Dictionary<string, Instance>, Exception> callback)
        {
            var request = new RestRequest
                              {
                                  Resource = string.Format("/apps/{0}/stats", appName),
                                  Method = Method.GET,
                                  RequestFormat = DataFormat.Json
                              };
            Execute(request, callback);
        }

        public void Logoff()
        {
            Authenticator = null;
        }

        private void Execute<T>(RestRequest request, Action<T, Exception> callback) where T : new()
        {
            var client = new RestClient
                             {
                                 BaseUrl = BaseUrl,
                                 Authenticator = Authenticator
                             };
            request.JsonSerializer = new JsonSerializer();
            client.ExecuteAsync<T>(request, response =>
                                                {
                                                    if (response.ErrorException != null)
                                                    {
                                                        callback(default(T), response.ErrorException);
                                                    }
                                                    else if (response.StatusCode != HttpStatusCode.OK)
                                                    {
                                                        callback(default(T), new Exception(response.StatusCode.ToString()));
                                                    }
                                                    else
                                                    {
                                                        callback(response.Data, null);
                                                    }
                                                });
        }

        private void Execute(RestRequest request, Action<bool, Exception> callback)
        {
            var client = new RestClient
                             {
                                 BaseUrl = BaseUrl,
                                 Authenticator = Authenticator
                             };
            request.JsonSerializer = new JsonSerializer();
            client.ExecuteAsync(request, response =>
                                             {
                                                 if (response.ErrorException != null)
                                                 {
                                                     callback(false, response.ErrorException);
                                                 }
                                                 else if (response.StatusCode != HttpStatusCode.OK)
                                                 {
                                                     callback(false, new Exception(response.StatusCode.ToString()));
                                                 }
                                                 else
                                                 {
                                                     callback(true, null);
                                                 }
                                             });
        }
    }
}