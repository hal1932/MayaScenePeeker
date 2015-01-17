using System.Windows;

namespace mayapeeker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new Views.MainWindow();
            mainWindow.Show();
        }


        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
