using System;
using System.Collections.Generic;
using System.Threading.Tasks;


using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureFoodProvider : IFoodProvider
	{
		#region Fields
		private readonly IAzureClient _azureClient;
		#endregion

		#region Ctor
		public AzureFoodProvider(IAzureClient azureClient)
		{
			_azureClient = azureClient;
		}
		#endregion

		#region IFoodProvider members
		public async Task<IEnumerable<FoodDescriptor>> Search(string title)
		{
			throw new NotImplementedException();
		}

		public async Task<FoodDescriptor> Lookup(string barcode, string barcodeType)
		{
			try
			{
				string blobContent = await _azureClient.GetBlobFromContainer("products", barcode);

				return blobContent != null ? new FoodDescriptor() : null;
			}
			catch (Microsoft.WindowsAzure.Storage.StorageException)
			{
				return null;
			}
		}
		#endregion

		#region Methods
		#endregion
	}
}