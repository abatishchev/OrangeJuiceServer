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
			string envelop = CreateEnvelop(RequestMethod, host, path, query);
			return SignEnvelop(envelop);
		}
		#endregion

		#region Methods
		private static string CreateEnvelop(params string[] strings)
		{
			return String.Join("\n", strings);
		}

		private string SignEnvelop(string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			byte[] hash = _hashAlgorithm.ComputeHash(bytes);
			return Convert.ToBase64String(hash);
		}
		#endregion
	}
}