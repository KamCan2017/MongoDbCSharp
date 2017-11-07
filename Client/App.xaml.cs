using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string error = e.Exception.Message;
            if (e.Exception.InnerException != null)
                error = e.Exception.InnerException.Message;

            var dialogres = Xceed.Wpf.Toolkit.MessageBox.Show(error, "Error", MessageBoxButton.OK);
            App.Current.Shutdown();
        }
    }
}
