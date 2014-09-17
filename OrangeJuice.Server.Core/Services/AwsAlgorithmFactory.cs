using System.Security.Cryptography;
using System.Text;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsAlgorithmFactory : Factory.IFactory<HashAlgorithm>
	{
		private readonly string _secretKey;

		public AwsAlgorithmFactory(AwsOptions awsOptions)
		{
			_secretKey = awsOptions.SecretKey;
		}

		public HashAlgorithm Create()
		{
			byte[] secret = Encoding.UTF8.GetBytes(_secretKey);
			return new HMACSHA256(secret);
		}
	}
}