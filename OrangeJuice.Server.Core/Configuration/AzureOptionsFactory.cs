namespace OrangeJuice.Server.Configuration
{
	public class AzureOptionsFactory : IFactory<AzureOptions>
	{
		// TODO: support environments!
		private const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=orangejuicedev;AccountKey=Hc+ThPW3d5Sokm2mMiIG8FCaoKpuUrncJlihwy7Gzf+Pu7b0fAa9NEmQMPKr48bdUnx3uKvyqJaKWj843RrwNw==";

		public AzureOptions Create()
		{
			return new AzureOptions
			{
				ConnectionString = ConnectionString
			};
		}
	}
}