using System;

namespace OrangeJuice.Server.Configuration
{
	public class AwsOptions
	{
		public string AccessKey { get; set; }

		public string AssociateTag { get; set; }

		public string SecretKey { get; set; }
		
		public TimeSpan RequestLimit { get; set; }
	}
}