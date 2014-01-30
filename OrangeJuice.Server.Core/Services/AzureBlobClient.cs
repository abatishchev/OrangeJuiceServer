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
			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
			{
				streamWriter.Write(content);
				await blob.UploadFromStreamAsync(stream);
			}
		}
		#endregion
	}
}