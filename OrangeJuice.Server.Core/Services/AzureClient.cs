using System;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureClient : IAzureClient
	{
		#region Fields
		// TODO: refactor down into BlobReader?
		private readonly IBlobNameResolver _blobNameResolver;
		private readonly IBlobReader _blobReader;
		#endregion

		#region Ctor
		public AzureClient(IBlobNameResolver blobNameResolver, IBlobReader blobReader)
		{
			_blobReader = blobReader;
			_blobNameResolver = blobNameResolver;
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
		private static async Task<CloudBlobContainer> GetContainer(string name)
		{
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=orangejuicedev;AccountKey=Hc+ThPW3d5Sokm2mMiIG8FCaoKpuUrncJlihwy7Gzf+Pu7b0fAa9NEmQMPKr48bdUnx3uKvyqJaKWj843RrwNw==");
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			CloudBlobContainer container = blobClient.GetContainerReference(name);
			bool exists = await container.ExistsAsync();
			if (!exists)
				throw new InvalidOperationException(String.Format("Blob {0} doesn't exist", name));

			return container;
		}
		#endregion
	}
}