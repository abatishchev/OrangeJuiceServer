using System;

namespace OrangeJuice.Server.Configuration
{
	public class AwsOptionsFactory : Factory.IFactory<AwsOptions>
	{
		private readonly IConfigurationProvider _configurationProvider;

		public AwsOptionsFactory(IConfigurationProvider configurationProvider)
		{
			_configurationProvider = configurationProvider;
		}

		public AwsOptions Create()
		{
			return new AwsOptions
			{
				AccessKey = _configurationProvider.GetValue("aws:AccessKey"),
				AssociateTag = _configurationProvider.GetValue("aws:AssociateTag"),
				SecretKey = _configurationProvider.GetValue("aws:SecretKey"),
				RequestLimit = TimeSpan.Parse(_configurationProvider.GetValue("aws:RequestLimit"))
			};
		}
	}
}