using System;
using System.Security.Cryptography;
using System.Text;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Api.Builders
{
	internal sealed class SignatureBuilder
	{
		private const string RequestEndpoint = "webservices.amazon.com";
		private const string RequestUri = "/onca/xml";
		private const string RequestMethod = "GET";

		private readonly HashAlgorithm _hashAlgorithm;
		private readonly IUrlEncoder _urlEncoder;

		public SignatureBuilder(string secretKey, IUrlEncoder urlEncoder)
			: this(CreateHashAlgorithm(secretKey), urlEncoder)
		{
		}

		public SignatureBuilder(HashAlgorithm hashAlgorithm, IUrlEncoder urlEncoder)
		{
			_hashAlgorithm = hashAlgorithm;
			_urlEncoder = urlEncoder;
		}

		private static HashAlgorithm CreateHashAlgorithm(string secretKey)
		{
			byte[] secret = Encoding.UTF8.GetBytes(secretKey);
			return new HMACSHA256(secret);
		}

		/// <summary>
		/// Signs a request in the form of a dictionary
		/// </summary>
		/// <returns>Returns a complete URL to use</returns>
		/// <remarks>Modifying the returned URL in any way invalidates the signature and Amazon will reject the requests</remarks>
		public string SignQuery(string query)
		{
			string signature = CreateSignature(query);

			StringBuilder sb = new StringBuilder();
			sb.Append(Uri.UriSchemeHttp)
						.Append("://")
						.Append(RequestEndpoint)
						.Append(RequestUri)
						.Append('?')
						.Append(query)
						.Append("&Signature=")
						.Append(_urlEncoder.Encode(signature));
			return sb.ToString();
		}

		private string CreateSignature(string queryString)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(RequestMethod)
						 .Append('\n')
						 .Append(RequestEndpoint)
						 .Append('\n')
						 .Append(RequestUri)
						 .Append('\n')
						 .Append(queryString);

			byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
			byte[] hash = _hashAlgorithm.ComputeHash(bytes);
			return Convert.ToBase64String(hash);
		}
	}
}