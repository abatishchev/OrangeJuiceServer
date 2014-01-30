using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureProductProvider : IProductProvider
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

		#region IProductProvider members
		public async Task Save(ProductDescriptor descriptor)
		{
			string content = _converter.Convert(descriptor);
			await _client.PutBlobToContainer(ProductContainerName, descriptor.ProductId.ToString(), content);
		}

		public async Task<ProductDescriptor> SearchId(Guid productId)
		{
			string content = await _client.GetBlobFromContainer(ProductContainerName, productId.ToString());
			return _converter.Convert(content);
		}

		public Task<IEnumerable<ProductDescriptor>> SearchTitle(string title)
		{
			throw new NotImplementedException();
		}

		public Task<ProductDescriptor> SearchBarcode(string barcode, BarcodeType barcodeType)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}