using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class QueryAppVersionHandler : AppVersionHandler
	{
		private readonly Version _appVersion;

		public QueryAppVersionHandler(Version appVersion)
		{
			_appVersion = appVersion;
		}

		internal override bool IsValid(HttpRequestMessage request)
		{
			return GetRules(request).All(b => b);
		}

		private IEnumerable<bool> GetRules(HttpRequestMessage request)
		{
			var query = request.RequestUri.ParseQueryString();
			string appVer = query["appVer"];

			Version version;
			yield return Version.TryParse(appVer, out version);

			yield return version == _appVersion;
		}
	}
}