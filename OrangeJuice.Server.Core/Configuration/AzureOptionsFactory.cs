namespace OrangeJuice.Server.Configuration
{
	public class AzureOptionsFactory : Factory.IFactory<AzureOptions>
	{
		private readonly IConfigurationProvider _configurationProvider;

		public AzureOptionsFactory(IConfigurationProvider configurationProvider)
		{
			_configurationProvider = configurationProvider;
		}

		public AzureOptions Create()
		{
			return new AzureOptions
			{
				ConnectionString = _configurationProvider.GetValue("azure:ConnectionString"),
				ProductsContainer = _configurationProvider.GetValue("azure:Products"),
				AwsOptionsContainer = _configurationProvider.GetValue("azure:AwsOptions"),
			};
		}
	}
}