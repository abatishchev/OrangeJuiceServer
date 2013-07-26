using System;
using System.Net;
using System.Net.Http;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class AppKeyQueryHandler : ValidatingDelegatingHandler
	{
		internal const string AppKeySegmentName = "appKey";

		private readonly Guid _appKey;

		public AppKeyQueryHandler(Guid appKey)
		{
			_appKey = appKey;
		}

		internal override HttpStatusCode ErrorCode
		{
			get { return HttpStatusCode.Forbidden; }
		}

		internal override bool IsValid(HttpRequestMessage request)
		{
			var query = request.RequestUri.ParseQueryString();
			string appKey = query[AppKeySegmentName];

			Guid guid;
			return Guid.TryParse(appKey, out guid) &&
				guid == _appKey;
		}
	}
}