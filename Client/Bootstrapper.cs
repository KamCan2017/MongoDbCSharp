using Client.Developer;
using Client.ViewModels;
using Client.Views;
using Core;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Prism.Events;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Windows;

namespace Client
{
    public class Bootstrapper : UnityBootstrapper
    {
        /// <summary>
        /// Lädt und konfiguriert den Container
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.AddNewExtension<Interception>();
            Container.RegisterType<IBusyIndicator, MainWindowViewModel>(new ContainerControlledLifetimeManager());
         
            RegisterTypeIfMissing(typeof(IEventAggregator), typeof(EventAggregator), true);

            ModuleManager moduleManager = Container.TryResolve<ModuleManager>();
            moduleManager.LoadModule("DeveloperModule");
        }

        /// <summary>
        /// Lädt und konfiguriert den ModuleCatalog
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(DeveloperModule));
        }

        /// <summary>
        /// Erzeugt eine neue Shell
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject CreateShell()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory(t =>
            {
                return Container.Resolve(t);
            });

            // Use the container to create an instance of the shell.
            MainWindow view = this.Container.TryResolve<MainWindow>();
            return view;
        }

        /// <summary>
        /// Initialisiert die Shell
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window)Shell;
            ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            Application.Current.MainWindow.Show();

            IRegionManager regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            regionManager.RequestNavigate("LeftRegion", new Uri("DeveloperList", UriKind.Relative));
            regionManager.RequestNavigate("RightRegion", new Uri("DeveloperView", UriKind.Relative));
        }
    }
}
