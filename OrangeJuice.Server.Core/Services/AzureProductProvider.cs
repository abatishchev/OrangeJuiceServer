using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureProductProvider : IAzureProductProvider
	{
		#region Fields
		private readonly AzureOptions _azureOptions;
		private readonly IAzureClient _client;
		private readonly IConverter<string, ProductDescriptor> _converter;
		#endregion

		#region Ctor
		public AzureProductProvider(AzureOptions azureOptions, IAzureClient client, IConverter<string, ProductDescriptor> converter)
		{
			_azureOptions = azureOptions;
			_client = client;
			_converter = converter;
		}
		#endregion

		#region IAzureProductProvider members
		public async Task<ProductDescriptor> Get(Guid productId)
		{
			string content = await _client.GetBlobFromContainer(_azureOptions.ProductContainer, productId.ToString());
			return _converter.Convert(content);
		}

		public async Task Save(ProductDescriptor descriptor)
		{
			string content = _converter.Convert(descriptor);
			await _client.PutBlobToContainer(_azureOptions.ProductContainer, descriptor.ProductId.ToString(), content);
		}

		#endregion
	}
}