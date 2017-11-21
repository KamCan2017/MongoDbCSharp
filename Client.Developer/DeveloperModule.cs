using Client.Developer.ViewModels;
using Client.Developer.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
namespace Client.Developer
{
    public class DeveloperModule : IModule
    {
        private readonly IUnityContainer _container;

        public DeveloperModule(IUnityContainer container)
        {
            _container = container;
        }
        public void Initialize()
        {
            _container.RegisterType<object, DeveloperView>("DeveloperView");
            _container.RegisterType<DeveloperViewModel>();

            _container.RegisterType<object, DeveloperList>("DeveloperList");
            _container.RegisterType<DeveloperListViewModel>();

            _container.RegisterType<object, KnowledgeEditor>("KnowledgeEditor");
            _container.RegisterType<KnowledgeEditorViewModel>();
        }
    }
}
