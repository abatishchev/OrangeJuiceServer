using Microsoft.Practices.Unity;

// ReSharper disable once CheckNamespace
namespace OrangeJuice.Server.Api
{
	internal static class UnityContainerExtensions
	{
		public static IUnityContainer RegisterFactory<T>(this IUnityContainer container, LifetimeManager lifetimeManager)
		{
			return container.RegisterType<T>(lifetimeManager, new InjectionFactory(c => c.Resolve<IFactory<T>>().Create()));
		}
	}
}