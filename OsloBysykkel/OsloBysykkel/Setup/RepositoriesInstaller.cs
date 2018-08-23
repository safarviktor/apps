using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using OsloBysykkel.DataAccess;

namespace OsloBysykkel.Setup
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromAssemblyNamed("OsloBysykkel.DataAccess")
                .Where(type => type.IsAssignableFrom(typeof(BaseRepository)) && !type.IsAbstract)
                .WithService.DefaultInterfaces()
                .LifestyleTransient());
        }
    }
}