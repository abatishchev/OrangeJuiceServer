using System.Collections.Generic;
using System.Net.Http;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	static class HandlerConfig
	{
		public static void ConfigurHandlers(ICollection<DelegatingHandler> handlers)
		{
			handlers.Add(new Handlers.AppKeyQueryHandler(AppKey.Version0));
		}
	}
}