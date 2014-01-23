using System.Security.Cryptography;
using System.Text;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsAlgorithmFactory : IFactory<HashAlgorithm>
	{
		private readonly string _secretKey;

		public AwsAlgorithmFactory(string secretKey)
		{
			_secretKey = secretKey;
		}

		public HashAlgorithm Create()
		{
			byte[] secret = Encoding.UTF8.GetBytes(_secretKey);
			return new HMACSHA256(secret);
		}
	}
}