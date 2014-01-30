using System;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureClient : IAzureClient
	{
		#region Fields
		private readonly AzureOptions _azureOptions;
		private readonly IBlobNameResolver _blobNameResolver;
		private readonly IBlobClient _blobClient;
		#endregion

		#region Ctor
		public AzureClient(AzureOptions azureOptions, IBlobNameResolver blobNameResolver, IBlobClient blobClient)
		{
			_azureOptions = azureOptions;
			_blobNameResolver = blobNameResolver;
			_blobClient = blobClient;
		}
		#endregion

		#region IAzureClient members

		public async Task<string> GetBlobFromContainer(string containerName, string fileName)
		{
			CloudBlobContainer container = await GetContainer(containerName);

			string blobName = _blobNameResolver.Resolve(fileName);
			ICloudBlob blob = await container.GetBlobReferenceFromServerAsync(blobName);

			bool exists = await blob.ExistsAsync();
			if (!exists)
				return null;

			return await _blobClient.Read(blob);
		}

		public async Task PutBlobToContainer(string containerName, string fileName, string content)
		{
			CloudBlobContainer container = await GetContainer(containerName);
			string blobName = _blobNameResolver.Resolve(fileName);

			ICloudBlob blob = container.GetBlockBlobReference(blobName);
			await _blobClient.Write(blob, content);
		}
		#endregion

		#region Methods
		// TODO: refactor out
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