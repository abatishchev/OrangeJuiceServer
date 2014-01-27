using System.Threading.Tasks;

namespace OrangeJuice.Server.Services
{
	public interface IAzureClient
	{
		Task<string> GetBlobFromContainer(string containerName, string blobName);
	}
}