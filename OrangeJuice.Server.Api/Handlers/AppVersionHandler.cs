using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Ab.Validation;

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

			return Task.FromResult(request.CreateResponse(HttpStatusCode.Forbidden));
		}
	}
}