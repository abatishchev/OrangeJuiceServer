using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Microsoft.Owin.Security.DataHandler.Encoder;

using Newtonsoft.Json;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Security
{
	public sealed class JwtFactory : Factory.IFactory<string>
	{
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		private readonly GoogleAuthOptions _authOptions;

		public JwtFactory(GoogleAuthOptions authOptions)
		{
			_authOptions = authOptions;
		}

		public string Create()
		{
			DateTime now = DateTime.UtcNow;
			var claimset = new
			{
				iss = _authOptions.Issuer,
				scope = "https://www.googleapis.com/auth/plus.me",
				aud = _authOptions.Audience,
				iat = ((int)now.Subtract(UnixEpoch).TotalSeconds).ToString(CultureInfo.InvariantCulture),
				exp = ((int)now.AddMinutes(55).Subtract(UnixEpoch).TotalSeconds).ToString(CultureInfo.InvariantCulture)
			};

			// header
			var header = new { typ = "JWT", alg = "RS256" };

			// encoded header
			var headerSerialized = JsonConvert.SerializeObject(header);
			var headerBytes = Encoding.UTF8.GetBytes(headerSerialized);
			var headerEncoded = TextEncodings.Base64Url.Encode(headerBytes);

			// encoded claimset
			var claimsetSerialized = JsonConvert.SerializeObject(claimset);
			var claimsetBytes = Encoding.UTF8.GetBytes(claimsetSerialized);
			var claimsetEncoded = TextEncodings.Base64Url.Encode(claimsetBytes);

			// input
			var input = String.Join(".", headerEncoded, claimsetEncoded);
			var inputBytes = Encoding.UTF8.GetBytes(input);

			// signature
			var signatureBytes = Sign(inputBytes);
			var signatureEncoded = TextEncodings.Base64Url.Encode(signatureBytes);

			// jwt
			return String.Join(".", headerEncoded, claimsetEncoded, signatureEncoded);
		}

		private byte[] Sign(byte[] inputBytes)
		{
			var certificate = new X509Certificate2(Convert.FromBase64String(_authOptions.CertificateKey), _authOptions.CertificateSecret);
			var rsa = (RSACryptoServiceProvider)certificate.PrivateKey;
			var cspParam = new CspParameters
			{
				KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName,
				KeyNumber = rsa.CspKeyContainerInfo.KeyNumber == KeyNumber.Exchange ? 1 : 2
			};
			var csp = new RSACryptoServiceProvider(cspParam) { PersistKeyInCsp = false };
			return csp.SignData(inputBytes, "SHA256");
		}
	}
}