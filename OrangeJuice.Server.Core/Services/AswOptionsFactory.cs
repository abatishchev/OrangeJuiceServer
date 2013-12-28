using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Services
{
	public class AswOptionsFactory
	{
		internal const string AwsAccessKey = "aws:AccessKey";
		internal const string AwsAssociateTag = "aws:AssociateTag";
		internal const string AwsSecretKey = "aws:SecretKey";

		private readonly IConfigurationProvider _configurationProvider;

		public AswOptionsFactory(IConfigurationProvider configurationProvider)
		{
			_configurationProvider = configurationProvider;
		}

		public AwsOptions Create()
		{
			return new AwsOptions
			{
				AccessKey = _configurationProvider.GetValue(AwsAccessKey),
				AssociateTag = _configurationProvider.GetValue(AwsAssociateTag),
				SecretKey = _configurationProvider.GetValue(AwsSecretKey)
			};
		}
	}
}