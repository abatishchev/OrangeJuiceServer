using System.IO;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

namespace OrangeJuice.Server.Services
{
	public sealed class AsyncStreamBlobReader : IBlobReader
	{
		public async Task<string> Read(ICloudBlob blob)
		{
			using (Stream stream = await blob.OpenReadAsync())
			using (StreamReader reader = new StreamReader(stream))
			{
				return await reader.ReadToEndAsync();
			}
		}
	}
}