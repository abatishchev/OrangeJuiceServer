using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using SimpleInjector;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class DelegatingHandlerProxy<THandler> : DelegatingHandler
		where THandler : DelegatingHandler
	{
		private readonly Container _container;

		public DelegatingHandlerProxy(Container container)
		{
			_container = container;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			request.GetDependencyScope();

			var handler = _container.GetInstance<THandler>();
			handler.InnerHandler = InnerHandler;

			var invoker = new HttpMessageInvoker(handler);
			return invoker.SendAsync(request, cancellationToken);
		}
	}
}