using System;
using System.Threading.Tasks;

using Ab;
using Ab.Amazon.Data;
using Ab.Azure;
using Ab.Azure.Configuration;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureProductProvider : IAzureProductProvider
	{
		#region Fields
		private readonly AzureOptions _azureOptions;
		private readonly IAzureClient _client;
		private readonly IConverter<string, Product> _converter;
		#endregion

		#region Ctor
		public AzureProductProvider(AzureOptions azureOptions, IAzureClient client, IConverter<string, Product> converter)
		{
			_azureOptions = azureOptions;
			_client = client;
			_converter = converter;
		}
		#endregion

		#region IAzureProductProvider members
		public async Task<Product> Get(Guid productId)
		{
			string content = await _client.GetBlobFromContainer(_azureOptions.ProductsContainer, productId.ToString());
			return content != null ? _converter.Convert(content) : null;
		}

		public Task Save(Product descriptor)
		{
			string content = _converter.ConvertBack(descriptor);
			return _client.PutBlobToContainer(_azureOptions.ProductsContainer, descriptor.ProductId.ToString(), content);
		}

		public Task<Uri> GetUrl(Guid productId)
		{
			return _client.GetBlobUrl(_azureOptions.ProductsContainer, productId.ToString());
		}
		#endregion
	}
}