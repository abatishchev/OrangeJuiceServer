using System;
using System.Net.Http;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class QueryAppKeyHandler : AppKeyHandlerBase
	{
		internal const string AppKeySegmentName = "appKey";

		private readonly Guid _appKey;

		public QueryAppKeyHandler(Guid appKey)
		{
			_appKey = appKey;
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