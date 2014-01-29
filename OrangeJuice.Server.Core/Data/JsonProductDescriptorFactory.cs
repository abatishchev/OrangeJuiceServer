using Newtonsoft.Json.Linq;

namespace OrangeJuice.Server.Data
{
	public sealed class JsonProductDescriptorFactory : IProductDescriptorFactory<string>
	{
		#region IProductDescriptorFactory members
		public ProductDescriptor Create(string content)
		{
			JObject jobj = JObject.Parse(content);
			return jobj.ToObject<ProductDescriptor>();
		}
		#endregion

	}
}