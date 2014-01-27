using System;

namespace OrangeJuice.Server.Services
{
	public sealed class JsonBlobNameResolver : IBlobNameResolver
	{
		public string Resolve(string blobName)
		{
			return String.Format("{0}.json", blobName);
		}
	}
}