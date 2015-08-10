using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureClient : IAzureClient
	{
		private static readonly TimeSpan Year = TimeSpan.FromDays(365);

		private readonly IBlobClient _blobClient;
		private readonly IAzureContainerClient _containerClient;

		public AzureClient(IBlobClient blobClient, IAzureContainerClient containerClient)
		{
			_blobClient = blobClient;
			_containerClient = containerClient;
		}

		public async Task<string> GetBlobFromContainer(string containerName, string fileName)
		{
			ICloudBlob blob = await _containerClient.GetBlockReference(containerName, fileName);

			bool exists = await blob.ExistsAsync();
			if (!exists)
				return null;

			return await _blobClient.Read(blob);
		}

		public async Task<string[]> GetBlobsFromContainer(string containerName)
		{
			var container = await _containerClient.GetContainer(containerName);
			var tasks = container.ListBlobs()
								 .Cast<CloudBlockBlob>()
								 .Select(b => b.DownloadTextAsync());
			return await Task.WhenAll(tasks);
		}

		public async Task PutBlobToContainer(string containerName, string fileName, string content)
		{
			ICloudBlob blob = await _containerClient.GetBlockReference(containerName, fileName);
			blob.Properties.CacheControl = CreateCacheControl(Year);
			await _blobClient.Write(blob, content);
		}

		public async Task<Uri> GetBlobUrl(string containerName, string fileName)
		{
			CloudBlob blob = await _containerClient.GetBlobReference(containerName, fileName);
			bool exists = await blob.ExistsAsync();
			return exists ? blob.Uri : null;
		}

		private static string CreateCacheControl(TimeSpan timeSpan)
		{
			return String.Format("public, max-age={0}", timeSpan.TotalMilliseconds);
		}
	}
}