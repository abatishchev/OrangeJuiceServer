using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class HeaderAppKeyHandler : AppKeyHandlerBase
	{
		internal const string AppKeyHeaderName = "X-ApiKey";

		private readonly Guid _appKey;

		public HeaderAppKeyHandler(Guid appKey)
		{
			_appKey = appKey;
		}

		internal override bool IsValid(HttpRequestMessage request)
		{
			return GetRules(request).All(b => b);
		}

		private IEnumerable<bool> GetRules(HttpRequestMessage request)
		{
			IEnumerable<string> values;
			yield return request.Headers.TryGetValues(AppKeyHeaderName, out values);

			Guid guid;
			yield return Guid.TryParse(values.FirstOrDefault(), out guid);

			yield return guid == _appKey;
		}
	}
}