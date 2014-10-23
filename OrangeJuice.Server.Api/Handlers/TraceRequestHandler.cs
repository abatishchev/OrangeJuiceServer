using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

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
			Task.Factory.StartNew(async () => await TraceRequest(request), cancellationToken);

			return base.SendAsync(request, cancellationToken);
		}

		private Task TraceRequest(HttpRequestMessage request)
		{
			return _repository.Add(
                DateTime.UtcNow,
                request.RequestUri.ToString(),
				request.Method.Method,
				((System.Web.HttpContextWrapper)request.Properties[MsHttpContextKey]).Request.UserHostAddress,
				request.Headers.UserAgent.ToString());
		}
	}
}