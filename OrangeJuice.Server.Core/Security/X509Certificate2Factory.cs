using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Security
{
	public sealed class X509Certificate2Factory : Factory.IFactory<X509Certificate2>
	{
		private readonly GoogleAuthOptions _authOptions;
		private readonly IEnvironmentProvider _environmentProvider;

		public X509Certificate2Factory(GoogleAuthOptions authOptions, IEnvironmentProvider environmentProvider)
		{
			_authOptions = authOptions;
			_environmentProvider = environmentProvider;
		}

		public X509Certificate2 Create()
		{
			switch (_environmentProvider.GetCurrentEnvironment())
			{
				case EnvironmentName.Production:
					return Create(_authOptions.CertificateThumbprint);
				default:
					return Create(_authOptions.CertificateKey, _authOptions.CertificateSecret);
			}
		}

		private static X509Certificate2 Create(string key, string secret)
		{
			return new X509Certificate2(Convert.FromBase64String(key), secret);
		}

		private static X509Certificate2 Create(string thumbprint)
		{
			var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			certStore.Open(OpenFlags.ReadOnly);

			var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
			var cert = certCollection.Cast<X509Certificate2>().FirstOrDefault();

			certStore.Close();

			if (cert == null)
			{
				throw new CryptographicException(String.Format("Certificate with thumbprint '{0}' was not found in store '{1}'", thumbprint, certStore.Name));
			}
			return cert;
		}
	}
}