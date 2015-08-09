using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

namespace OrangeJuice.Server.Services
{
	public interface IAzureContainerClient
	{
		Task<ICloudBlob> GetBlobReference(string containerName, string blobName);

		Task<CloudBlockBlob> GetBlockReference(string containerName, string blobName);

		Task<CloudBlobContainer> GetContainer(string containerName);
	}
}