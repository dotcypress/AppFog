#region

using System;
using System.Windows.Input;
using Electrum.Logging;
using Microsoft.Phone.Tasks;
using dotCypress.AppFog.Localization;

#endregion

namespace dotCypress.AppFog.App.Core.Commands
{
    public class SendFeedbackCommand : ICommand
    {
        private readonly ILogger _logger;
        private readonly string _supportEmail;

        public SendFeedbackCommand(string supportEmail, ILogger logger)
        {
            _supportEmail = supportEmail;
            _logger = logger;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _logger.Trace("SendFeedbackCommand executed");
            var task = new EmailComposeTask
                           {
                               To = _supportEmail,
                               Subject = AppResources.AppTitle + " WP7"
                           };
            task.Show();
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }

        #endregion
    }
}