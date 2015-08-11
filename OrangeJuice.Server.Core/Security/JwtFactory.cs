using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Factory;

using Microsoft.Owin.Security.DataHandler.Encoder;

using Newtonsoft.Json;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Security
{
	public sealed class JwtFactory : IFactory<Jwt>
	{
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		private readonly GoogleAuthOptions _authOptions;
		private readonly IFactory<X509Certificate2> _certFactory;

		public JwtFactory(GoogleAuthOptions authOptions, IFactory<X509Certificate2> certFactory)
		{
			_authOptions = authOptions;
			_certFactory = certFactory;
		}

		public Jwt Create()
		{
			var header = new { typ = "JWT", alg = "RS256" };
			var headerEncoded = Encode(header);

			DateTime now = DateTime.UtcNow;
			var claimset = new
			{
				iss = _authOptions.Issuer,
				scope = "https://www.googleapis.com/auth/plus.me",
				aud = _authOptions.Audience,
				iat = ((int)now.Subtract(UnixEpoch).TotalSeconds).ToString(CultureInfo.InvariantCulture),
				exp = ((int)now.AddMinutes(55).Subtract(UnixEpoch).TotalSeconds).ToString(CultureInfo.InvariantCulture)
			};
			var claimsetEncoded = Encode(claimset);

			var signature = CreateSignature(headerEncoded, claimsetEncoded);
			var signatureEncoded = TextEncodings.Base64Url.Encode(signature);

			return new Jwt
			{
				Value = String.Join(".", headerEncoded, claimsetEncoded, signatureEncoded)
			};
		}

		private static string Encode(object value)
		{
			string serialized = JsonConvert.SerializeObject(value);
			byte[] bytes = Encoding.UTF8.GetBytes(serialized);
			return TextEncodings.Base64Url.Encode(bytes);
		}

		private byte[] CreateSignature(string headerEncoded, string claimsetEncoded)
		{
			var input = String.Join(".", headerEncoded, claimsetEncoded);
			var inputBytes = Encoding.UTF8.GetBytes(input);
			return Sign(inputBytes);
		}

		private byte[] Sign(byte[] bytes)
		{
			var certificate = _certFactory.Create();
			var rsa = (RSACryptoServiceProvider)certificate.PrivateKey;
			var cspParam = new CspParameters
			{
				KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName,
				KeyNumber = rsa.CspKeyContainerInfo.KeyNumber == KeyNumber.Exchange ? 1 : 2
			};
			var csp = new RSACryptoServiceProvider(cspParam) { PersistKeyInCsp = false };
			return csp.SignData(bytes, "SHA256");
		}
	}
}