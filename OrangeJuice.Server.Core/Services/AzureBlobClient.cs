using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureBlobClient : IBlobClient
	{
		#region IBlobClient members
		public async Task<string> Read(ICloudBlob blob)
		{
			using (Stream stream = await blob.OpenReadAsync())
			using (StreamReader reader = new StreamReader(stream))
			{
				return await reader.ReadToEndAsync();
			}
		}

		public async Task Write(ICloudBlob blob, string content)
		{
			using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
			{
				await blob.UploadFromStreamAsync(stream);
			}
		}
		#endregion
	}
}