using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace OrangeJuice.Server.Services
{
	public interface IAzureContainerClient
	{
		Task<CloudBlobContainer> GetContainer(string containerName);

		Task<CloudBlob> GetBlobReference(string containerName, string blobName);

		Task<CloudBlockBlob> GetBlockReference(string containerName, string blobName);

		Task<CloudTable> GetTableReference(string tableName);
	}
}