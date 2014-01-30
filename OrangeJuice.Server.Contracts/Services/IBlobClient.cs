using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

namespace OrangeJuice.Server.Services
{
	public interface IBlobClient
	{
		Task<string> Read(ICloudBlob blob);

		Task Write(ICloudBlob blob, string content);
	}
}