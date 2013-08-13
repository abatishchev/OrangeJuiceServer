using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Api.Handlers
{
	public abstract class AppKeyHandlerBase : DelegatingHandler
	{
		internal abstract System.Net.HttpStatusCode ErrorCode { get; }

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (IsValid(request))
				return base.SendAsync(request, cancellationToken);

			HttpResponseMessage response = new HttpResponseMessage(ErrorCode);
			var tsc = new TaskCompletionSource<HttpResponseMessage>();
			tsc.SetResult(response);
			return tsc.Task;
		}

		internal abstract bool IsValid(HttpRequestMessage request);

		internal virtual Task<HttpResponseMessage> RequestIsValid(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return base.SendAsync(request, cancellationToken);
		}
	}
}