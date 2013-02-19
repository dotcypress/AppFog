#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#endregion

namespace dotCypress.AppFog.Common
{
    public class AppState : DependencyObject
    {
        private readonly List<TaskHolder> _tokens = new List<TaskHolder>();
        private readonly object _syncRoot = new object();

        #region Dependency registration

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy",
                                        typeof (bool),
                                        typeof (AppState),
                                        new PropertyMetadata(false));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                                        typeof (string),
                                        typeof (AppState),
                                        new PropertyMetadata(null));

        #endregion

        #region Public members

        public void ShowIndeterminate(string message, string token)
        {
            if (Deployment.Current.Dispatcher.CheckAccess())
            {
                ShowIndeterminateInternal(message, token);
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => ShowIndeterminateInternal(message, token));
            }
        }

        public void Hide(string token)
        {
            if (Deployment.Current.Dispatcher.CheckAccess())
            {
                HideInternal(token);
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => HideInternal(token));
            }
        }

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return (bool) GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region Private members

        private void ShowIndeterminateInternal(string message, string token)
        {
            if (token == null)
            {
                throw new ArgumentException("Token is null");
            }
            lock (_syncRoot)
            {
                if (_tokens.All(x => x.Token != token))
                {
                    _tokens.Add(new TaskHolder
                                    {
                                        Token = token,
                                        Message = message
                                    });
                }
                Update();
            }
        }

        private void HideInternal(string token)
        {
            if (token == null)
            {
                throw new ArgumentException("Token is null");
            }
            lock (_syncRoot)
            {
                var existing = _tokens.FirstOrDefault(x => x.Token == token);
                if (existing != null)
                {
                    _tokens.Remove(existing);
                }
                Update();
            }
        }

        private void Update()
        {
            var last = _tokens.LastOrDefault();
            if (last != null)
            {
                Text = last.Message;
                IsBusy = true;
            }
            else
            {
                Text = null;
                IsBusy = false;
            }
        }

        #endregion

        private class TaskHolder
        {
            public string Token { get; set; }
            public string Message { get; set; }
        }
    }
}