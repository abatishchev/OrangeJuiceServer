using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OrangeJuice.Server.Configuration
{
	public sealed class JsonAwsOptionsConverter : IConverter<string, AwsOptions>
	{
		public AwsOptions Convert(string value)
		{
			return JObject.Parse(value)
						  .ToObject<AwsOptions>();
		}

		public string ConvertBack(AwsOptions value)
		{
			return JObject.FromObject(value)
						  .ToString(Formatting.None);
		}
	}
}