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
				ConnectionString = _configurationProvider.GetValue("blobConnectionString"),
				ProductsContainer = _configurationProvider.GetValue("blobProducts"),
			};
		}
	}
}