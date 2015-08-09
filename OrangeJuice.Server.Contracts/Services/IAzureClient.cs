using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Services
{
	public interface IAzureClient
	{
		Task<string> GetBlobFromContainer(string containerName, string fileName);

		Task<IEnumerable<string>> GetBlobsFromContainer(string containerName);

		Task PutBlobToContainer(string containerName, string fileName, string content);

		Task<Uri> GetBlobUrl(string containerName, string fileName);
	}
}