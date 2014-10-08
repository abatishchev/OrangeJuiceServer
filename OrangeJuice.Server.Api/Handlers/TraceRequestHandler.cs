using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class TraceRequestHandler : DelegatingHandler
	{
		private const string MsHttpContextKey = "MS_HttpContext";

		private readonly ITraceRequestRepository _repository;

		public TraceRequestHandler(ITraceRequestRepository repository)
		{
			_repository = repository;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			Task.Factory.StartNew(() => TraceRequest(request), cancellationToken);

			return base.SendAsync(request, cancellationToken);
		}

		private void TraceRequest(HttpRequestMessage request)
		{
			_repository.Add(request.RequestUri.ToString(),
				request.Method.Method,
				((System.Web.HttpContextWrapper)request.Properties[MsHttpContextKey]).Request.UserHostAddress,
				request.Headers.UserAgent.ToString());
		}
	}
}