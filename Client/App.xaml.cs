using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string error = e.Exception.Message;
            if (e.Exception.InnerException != null)
                error = e.Exception.InnerException.Message;

            error += "\r\nThe application will be closed.";

            var dialogres = Xceed.Wpf.Toolkit.MessageBox.Show(error, "Error", MessageBoxButton.OK);

            App.Current.MainWindow.Close();
            App.Current.Shutdown();
        }
    }
}
