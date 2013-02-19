#region

using System;
using System.Windows.Input;
using Electrum.Logging;
using Electrum.Navigation;
using dotCypress.AppFog.App.Core.Navigation;

#endregion

namespace dotCypress.AppFog.App.Core.Commands
{
    public class OpenAboutCommand : ICommand
    {
        private readonly ILogger _logger;
        private readonly INavigationManager _navigationManager;

        public OpenAboutCommand(INavigationManager navigationManager, ILogger logger)
        {
            _navigationManager = navigationManager;
            _logger = logger;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _logger.Trace("OpenAboutCommand executed");
            _navigationManager.GoToAboutPage();
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }

        #endregion
    }
}