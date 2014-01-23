using System;
using System.Security.Cryptography;
using System.Text;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsQuerySigner : IQuerySigner
	{
		#region Constants
		private const string RequestMethod = "GET";
		#endregion

		#region Fields
		private readonly HashAlgorithm _hashAlgorithm;
		#endregion

		#region Ctor
		public AwsQuerySigner(HashAlgorithm hashAlgorithm)
		{
			_hashAlgorithm = hashAlgorithm;
		}
		#endregion

		#region IQuerySigner members
		/// <summary>
		/// Signs a request in the form of a dictionary
		/// </summary>
		/// <returns>Returns a complete URL to use</returns>
		/// <remarks>Modifying the returned URL invalidates the signature and Amazon will reject a request</remarks>
		public string CreateSignature(string host, string path, string query)
		{
			query = PrepareQuery(host, path, query);

			return CreateSignature(query);
		}
		#endregion

		#region Methods
		private static string PrepareQuery(string host, string path, string query)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(RequestMethod)
			  .Append('\n')
			  .Append(host)
			  .Append('\n')
			  .Append(path)
			  .Append('\n')
			  .Append(query);
			return sb.ToString();
		}

		private string CreateSignature(string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			byte[] hash = _hashAlgorithm.ComputeHash(bytes);
			return Convert.ToBase64String(hash);
		}
		#endregion
	}
}