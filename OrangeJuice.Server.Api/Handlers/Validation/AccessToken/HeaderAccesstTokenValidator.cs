using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace OrangeJuice.Server.Api.Handlers.Validation
{
	public sealed class HeaderAccesstTokenValidator : RulesValidator<HttpRequestMessage, string>
	{
		private static readonly Regex BearerTokenRegex = new Regex("Bearer (.+)", RegexOptions.Compiled);

		protected override IEnumerable<bool> GetRules(HttpRequestMessage request)
		{
			IEnumerable<string> values;
			yield return request.Headers.TryGetValues("Authorization", out values);

			Match match = BearerTokenRegex.Match(values.Single());

			ValidationResult = match.Groups[1].Value;
		}
	}
}