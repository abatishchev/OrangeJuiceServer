using Newtonsoft.Json.Linq;

namespace OrangeJuice.Server.Data
{
	public sealed class StringProductDescriptorConverter : IConverter<string, ProductDescriptor>
	{
		#region IConverter members
		public ProductDescriptor Convert(string content)
		{
			JObject jobj = JObject.Parse(content);
			return jobj.ToObject<ProductDescriptor>();
		}

		public string Convert(ProductDescriptor descriptor)
		{
			JObject jobj = JObject.FromObject(descriptor);
			return jobj.ToString();
		}
		#endregion
	}
}