namespace OrangeJuice.Server.Configuration
{
	public class GoogleAuthOptions
	{
		public string Audience { get; set; }

		public string CertificateKey { get; set; }

		public string CertificateSecret { get; set; }

		public string ClientId { get; set; }

		/// <remarks>Email</remarks>>
		public string Issuer { get; set; }

		public string CertificateThumbprint { get; set; }
	}
}