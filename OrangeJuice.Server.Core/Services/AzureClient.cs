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
		private static readonly TimeSpan Year = TimeSpan.FromDays(365);

		private readonly AzureOptions _azureOptions;
		private readonly IBlobClient _blobClient;
		#endregion

		#region Ctor
		public AzureClient(AzureOptions azureOptions, IBlobClient blobClient)
		{
			_azureOptions = azureOptions;
			_blobClient = blobClient;
		}
		#endregion

		#region IAzureClient members

		public async Task<string> GetBlobFromContainer(string containerName, string fileName)
		{
			ICloudBlob blob = await GetBlockReference(containerName, fileName);

			bool exists = await blob.ExistsAsync();
			if (!exists)
				return null;

			return await _blobClient.Read(blob);
		}

		public async Task PutBlobToContainer(string containerName, string fileName, string content)
		{
			ICloudBlob blob = await GetBlockReference(containerName, fileName);
			blob.Properties.CacheControl = CreateCacheControl(Year);
			await _blobClient.Write(blob, content);
		}

		public async Task<Uri> GetBlobUrl(string containerName, string fileName)
		{
			ICloudBlob blob = await GetBlobReference(containerName, fileName);
			return blob.Uri;
		}
		#endregion

		#region Methods
		// TODO: refactor out?
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

		private async Task<ICloudBlob> GetBlobReference(string containerName, string blobName)
		{
			CloudBlobContainer container = await GetContainer(containerName);
			string fileName = CreateFileName(blobName);
			return container.GetBlobReferenceFromServer(fileName);
		}

		private async Task<CloudBlockBlob> GetBlockReference(string containerName, string blobName)
		{
			CloudBlobContainer container = await GetContainer(containerName);
			string fileName = CreateFileName(blobName);
			return container.GetBlockBlobReference(fileName);
		}

		private static string CreateCacheControl(TimeSpan timeSpan)
		{
			return String.Format("public, max-age={0}", timeSpan.TotalMilliseconds);
		}

		private static string CreateFileName(string blobName)
		{
			return String.Format("{0}.json", blobName);
		}
		#endregion
	}
}