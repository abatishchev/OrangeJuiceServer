using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Api.Handlers
{
	public abstract class AppKeyHandlerBase : DelegatingHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (IsValid(request))
				return base.SendAsync(request, cancellationToken);

			HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Forbidden);
			var tsc = new TaskCompletionSource<HttpResponseMessage>();
			tsc.SetResult(response);
			return tsc.Task;
		}

		internal abstract bool IsValid(HttpRequestMessage request);
	}
}