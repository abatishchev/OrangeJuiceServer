using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Api.Handlers
{
	public class AppVersionHandler : DelegatingHandler
	{
		private readonly IValidator<HttpRequestMessage> _requestValidator;

		public AppVersionHandler(IValidator<HttpRequestMessage> requestValidator)
		{
			_requestValidator = requestValidator;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (_requestValidator.IsValid(request))
				return base.SendAsync(request, cancellationToken);

			var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
			var tsc = new TaskCompletionSource<HttpResponseMessage>();
			tsc.SetResult(response);
			return tsc.Task;
		}
	}
}