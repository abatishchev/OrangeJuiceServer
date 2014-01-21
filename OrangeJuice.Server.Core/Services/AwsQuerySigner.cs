using System;
using System.Security.Cryptography;
using System.Text;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsQuerySigner : IQuerySigner
	{
		#region Constants
		private const string RequestMethod = "GET";
		#endregion

		#region Fields
		private readonly HashAlgorithm _hashAlgorithm;
		private readonly IUrlEncoder _urlEncoder;
		#endregion

		#region Ctor
		public AwsQuerySigner(string secretKey, IUrlEncoder urlEncoder)
			: this(CreateHashAlgorithm(secretKey), urlEncoder)
		{
		}

		public AwsQuerySigner(HashAlgorithm hashAlgorithm, IUrlEncoder urlEncoder)
		{
			_hashAlgorithm = hashAlgorithm;
			_urlEncoder = urlEncoder;
		}
		#endregion

		#region IQuerySigner members
		/// <summary>
		/// Signs a request in the form of a dictionary
		/// </summary>
		/// <returns>Returns a complete URL to use</returns>
		/// <remarks>Modifying the returned URL invalidates the signature and Amazon will reject a request</remarks>
		public string SignQuery(string host, string path, string query)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(RequestMethod)
			  .Append('\n')
			  .Append(host)
			  .Append('\n')
			  .Append(path)
			  .Append('\n')
			  .Append(query);

			string signature = CreateSignature(sb);
			return _urlEncoder.Encode(signature);
		}
		#endregion

		#region Methods
		internal static HashAlgorithm CreateHashAlgorithm(string secretKey)
		{
			byte[] secret = Encoding.UTF8.GetBytes(secretKey);
			return new HMACSHA256(secret);
		}

		private string CreateSignature(StringBuilder sb)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
			byte[] hash = _hashAlgorithm.ComputeHash(bytes);
			string signature = Convert.ToBase64String(hash);
			return signature;
		}
		#endregion
	}
}