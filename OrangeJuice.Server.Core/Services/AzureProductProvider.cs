using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureProductProvider : IAzureProductProvider
	{
		private const string ProductContainerName = "products";

		#region Fields
		private readonly IAzureClient _client;
		private readonly IConverter<string, ProductDescriptor> _converter;
		#endregion

		#region Ctor
		public AzureProductProvider(IAzureClient client, IConverter<string, ProductDescriptor> converter)
		{
			_client = client;
			_converter = converter;
		}
		#endregion

		#region IAzureProductProvider members
		public async Task<ProductDescriptor> Get(Guid productId)
		{
			string content = await _client.GetBlobFromContainer(ProductContainerName, productId.ToString());
			return _converter.Convert(content);
		}

		public async Task Save(ProductDescriptor descriptor)
		{
			string content = _converter.Convert(descriptor);
			await _client.PutBlobToContainer(ProductContainerName, descriptor.ProductId.ToString(), content);
		}

		#endregion
	}
}