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
			string content = await _client.GetBlobFromContainer(_azureOptions.ProductsContainer, productId.ToString());
			return content != null ? _converter.Convert(content) : null;
		}

		public async Task Save(ProductDescriptor descriptor)
		{
			string content = _converter.ConvertBack(descriptor);
			await _client.PutBlobToContainer(_azureOptions.ProductsContainer, descriptor.ProductId.ToString(), content);
		}

		public Uri GetUrl(Guid productId)
		{
			return _client.GetBlobUrl(_azureOptions.ProductsContainer, productId.ToString());
		}
		#endregion
	}
}