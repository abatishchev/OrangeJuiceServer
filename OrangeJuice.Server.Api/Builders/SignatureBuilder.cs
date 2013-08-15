using System;
using System.Security.Cryptography;
using System.Text;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Api.Builders
{
	internal sealed class SignatureBuilder
	{
		internal const string RequestEndpoint = "webservices.amazon.com";
		internal const string RequestPath = "/onca/xml";
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

		internal static HashAlgorithm CreateHashAlgorithm(string secretKey)
		{
			byte[] secret = Encoding.UTF8.GetBytes(secretKey);
			return new HMACSHA256(secret);
		}

		/// <summary>
		/// Signs a request in the form of a dictionary
		/// </summary>
		/// <returns>Returns a complete URL to use</returns>
		/// <remarks>Modifying the returned URL invalidates the signature and Amazon will reject a request</remarks>
		public string SignQuery(string query)
		{
			string signature = CreateSignature(query);

			StringBuilder sb = new StringBuilder();
			sb.Append(Uri.UriSchemeHttp)
			  .Append("://")
			  .Append(RequestEndpoint)
			  .Append(RequestPath)
			  .Append('?')
			  .Append(query)
			  .Append("&Signature=")
			  .Append(_urlEncoder.Encode(signature));
			return sb.ToString();
		}

		internal string CreateSignature(string queryString)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(RequestMethod)
			  .Append('\n')
			  .Append(RequestEndpoint)
			  .Append('\n')
			  .Append(RequestPath)
			  .Append('\n')
			  .Append(queryString);

			byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
			byte[] hash = _hashAlgorithm.ComputeHash(bytes);
			return Convert.ToBase64String(hash);
		}
	}
}