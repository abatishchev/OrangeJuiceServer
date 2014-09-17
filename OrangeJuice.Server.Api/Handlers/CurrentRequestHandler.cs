using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Api.Handlers
{
	public class CurrentRequest
	{
		public HttpRequestMessage Value { get; set; }
	}

	public class CurrentRequestHandler : DelegatingHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var scope = request.GetDependencyScope();
			var currentRequest = (CurrentRequest)scope.GetService(typeof(CurrentRequest));
			currentRequest.Value = request;
			return base.SendAsync(request, cancellationToken);
		}
	}
}