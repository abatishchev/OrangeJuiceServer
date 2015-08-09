using System;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureContainerClient : IAzureContainerClient
	{
		private readonly AzureOptions _azureOptions;

		public AzureContainerClient(AzureOptions azureOptions)
		{
			_azureOptions = azureOptions;
		}

		public async Task<CloudBlobContainer> GetContainer(string containerName)
		{
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_azureOptions.ConnectionString);
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			CloudBlobContainer container = blobClient.GetContainerReference(containerName);
			bool exists = await container.ExistsAsync();
			if (!exists)
				throw new InvalidOperationException(String.Format("Container {0} doesn't exist", containerName));

			return container;
		}

		public async Task<ICloudBlob> GetBlobReference(string containerName, string blobName)
		{
			CloudBlobContainer container = await GetContainer(containerName);
			string fileName = CreateFileName(blobName);
			return container.GetBlobReferenceFromServer(fileName);
		}

		public async Task<CloudBlockBlob> GetBlockReference(string containerName, string blobName)
		{
			CloudBlobContainer container = await GetContainer(containerName);
			string fileName = CreateFileName(blobName);
			return container.GetBlockBlobReference(fileName);
		}

		public async Task<Uri> GetBlobUrl(string containerName, string fileName)
		{
			ICloudBlob blob = await GetBlobReference(containerName, fileName);
			return blob.Uri;
		}

		private static string CreateFileName(string blobName)
		{
			return String.Format("{0}.json", blobName);
		}
	}
}