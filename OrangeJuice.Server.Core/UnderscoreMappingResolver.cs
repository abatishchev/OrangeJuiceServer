using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;

namespace OrangeJuice.Server
{
	public sealed class UnderscoreMappingResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
	{
		protected override string ResolvePropertyName(string propertyName)
		{
			return Regex.Replace(propertyName, "([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", "$1$3_$2$4", RegexOptions.Compiled).ToLower();
		}

		public static readonly IEnumerable<MediaTypeFormatter> Formatters = new[]
		{
			new JsonMediaTypeFormatter
			{
				SerializerSettings =
				{
					ContractResolver = new UnderscoreMappingResolver()
				}
			}
		};
	}
}