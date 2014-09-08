using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace OrangeJuice.Server.Api.Handlers.Validation
{
	public sealed class HeaderAppVersionValidator : IValidator<HttpRequestMessage>
	{
		private readonly Version _appVersion;

		public HeaderAppVersionValidator(Version appVersion)
		{
			_appVersion = appVersion;
		}

		public bool IsValid(HttpRequestMessage request)
		{
			return GetRules(request).All(b => b);
		}

		private IEnumerable<bool> GetRules(HttpRequestMessage request)
		{
			IEnumerable<string> values;
			yield return request.Headers.TryGetValues(AppVersion.ElementName, out values);

			Version version;
			yield return Version.TryParse(values.FirstOrDefault(), out version);

			yield return version == _appVersion;
		}
	}
}