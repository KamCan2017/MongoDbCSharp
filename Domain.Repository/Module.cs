using Microsoft.Practices.Unity;
using Prism.Modularity;
using Repository;


namespace Domain.Repository
{
    public class RespositoryModule : IModule
    {
        private readonly IUnityContainer _container;

        public RespositoryModule(IUnityContainer container)
        {
            _container = container;
        }
        public void Initialize()
        {
            _container.RegisterType<IDeveloperRepository, DeveloperRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IKnowledgeRepository, KnowledgeRepository>(new ContainerControlledLifetimeManager());

        }
    }
}
