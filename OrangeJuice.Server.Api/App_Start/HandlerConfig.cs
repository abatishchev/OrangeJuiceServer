using System.Collections.Generic;
using System.Net.Http;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Api.Handlers;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	internal static class HandlerConfig
	{
		public static void ConfigurHandlers(IUnityContainer container, ICollection<DelegatingHandler> handlers)
		{
			DelegatingHandler handler = container.Resolve<AppKeyHandlerBase>();
			if (handler != null)
				handlers.Add(handler);
		}
	}
}