using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Services
{
	public interface IAzureClient
	{
		Task<string> GetBlobFromContainer(string containerName, string fileName);

		Task PutBlobToContainer(string containerName, string fileName, string content);

		Uri GetBlobUrl(string fileName, string containerName);
	}
}