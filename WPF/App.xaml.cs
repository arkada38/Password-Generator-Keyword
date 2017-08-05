using System.Windows;
using WPF.Views;

namespace WPF
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new MainView().Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            WPF.Properties.Settings.Default.Save();
            base.OnExit(e);
        }
    }
}
