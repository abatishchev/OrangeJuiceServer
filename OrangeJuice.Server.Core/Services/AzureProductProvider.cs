using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureProductProvider : IProductProvider
	{
		#region Fields
		private readonly IAzureClient _client;
		private readonly IProductDescriptorFactory<string> _factory;
		#endregion

		#region Ctor
		public AzureProductProvider(IAzureClient client, IProductDescriptorFactory<string> factory)
		{
			_client = client;
			_factory = factory;
		}
		#endregion

		#region IProductProvider members
		public Task<IEnumerable<ProductDescriptor>> SearchTitle(string title)
		{
			throw new NotImplementedException();
		}

		public async Task<ProductDescriptor> SearchBarcode(string barcode, string barcodeType)
		{
			try
			{
				string blobContent = await _client.GetBlobFromContainer("products", barcode);
				return _factory.Create(blobContent);
			}
			catch (Microsoft.WindowsAzure.Storage.StorageException)
			{
				return null;
			}
		}
		#endregion
	}
}