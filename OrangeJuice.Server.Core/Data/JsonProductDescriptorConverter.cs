using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OrangeJuice.Server.Data
{
	public sealed class JsonProductDescriptorConverter : IConverter<string, ProductDescriptor>
	{
		#region IConverter members
		public ProductDescriptor Convert(string value)
		{
			return JObject.Parse(value)
			              .ToObject<ProductDescriptor>();
		}

		public string ConvertBack(ProductDescriptor value)
		{
			return JObject.FromObject(value)
			              .ToString(Formatting.Indented);
		}
		#endregion
	}
}