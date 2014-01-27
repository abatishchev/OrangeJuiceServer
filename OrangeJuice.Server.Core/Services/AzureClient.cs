using System;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Services
{
	// TODO: sepatate AzureClient from AzureBlobClient?
	public sealed class AzureClient : IAzureClient
	{
		#region Fields
		private readonly AzureOptions _azureOptions;
		private readonly IBlobNameResolver _blobNameResolver;
		private readonly IBlobReader _blobReader;
		#endregion

		#region Ctor
		public AzureClient(AzureOptions azureOptions, IBlobNameResolver blobNameResolver, IBlobReader blobReader)
		{
			_azureOptions = azureOptions;
			_blobNameResolver = blobNameResolver;
			_blobReader = blobReader;
		}
		#endregion

		#region IAzureClient members

		public async Task<string> GetBlobFromContainer(string containerName, string blobName)
		{
			CloudBlobContainer container = await GetContainer(containerName);

			string fileName = _blobNameResolver.Resolve(blobName);
			ICloudBlob blob = await container.GetBlobReferenceFromServerAsync(fileName);

			return await _blobReader.Read(blob);
		}

		#endregion

		#region Methods
		private async Task<CloudBlobContainer> GetContainer(string containerName)
		{
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_azureOptions.ConnectionString);
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			CloudBlobContainer container = blobClient.GetContainerReference(containerName);
			bool exists = await container.ExistsAsync();
			if (!exists)
				throw new InvalidOperationException(String.Format("Container {0} doesn't exist", containerName));

			return container;
		}
		#endregion
	}
}