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
			ICloudBlob blob = GetBlockReference(containerName, fileName);

			bool exists = blob.Exists();
			if (!exists)
				return null;

			return await _blobClient.Read(blob);
		}

		public async Task PutBlobToContainer(string containerName, string fileName, string content)
		{
			ICloudBlob blob = GetBlockReference(containerName, fileName);
			blob.Properties.CacheControl = CreateCacheControl(TimeSpan.FromDays(365));
			await _blobClient.Write(blob, content);
		}

		public Uri GetBlobUrl(string containerName, string fileName)
		{
			ICloudBlob blob = GetBlobReference(containerName, fileName);
			return blob.Uri;
		}
		#endregion

		#region Methods
		// TODO: refactor out?
		private CloudBlobContainer GetContainer(string containerName)
		{
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_azureOptions.ConnectionString);
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			CloudBlobContainer container = blobClient.GetContainerReference(containerName);
			bool exists = container.Exists();
			if (!exists)
				throw new InvalidOperationException(String.Format("Container {0} doesn't exist", containerName));

			return container;
		}

		private ICloudBlob GetBlobReference(string containerName, string fileName)
		{
			CloudBlobContainer container = GetContainer(containerName);
			string blobName = _blobNameResolver.Resolve(fileName);
			return container.GetBlobReferenceFromServer(blobName);
		}

		private CloudBlockBlob GetBlockReference(string containerName, string fileName)
		{
			CloudBlobContainer container = GetContainer(containerName);
			string blobName = _blobNameResolver.Resolve(fileName);
			return container.GetBlockBlobReference(blobName);
		}

		private static string CreateCacheControl(TimeSpan timeSpan)
		{
			return String.Format("public, max-age={0}", timeSpan.TotalMilliseconds);
		}
		#endregion
	}
}