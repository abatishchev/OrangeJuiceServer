using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

namespace OrangeJuice.Server.Services
{
	public interface IBlobReader
	{
		Task<string> Read(ICloudBlob blob);
	}
}