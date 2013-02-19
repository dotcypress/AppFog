#region

using Electrum.Navigation;

#endregion

namespace dotCypress.AppFog.App.Core.Navigation
{
    public static class NavigationManagerExtensions
    {
        public static void GoToAboutPage(this INavigationManager manager)
        {
            manager.Navigate("/Views/AboutPage.xaml");
        }

        public static void GoToApplicationPage(this INavigationManager manager, string applicationName)
        {
            manager.Navigate("/Views/AppInfoPage.xaml", new PageQuery {{"name", applicationName}});
        }
    }
}