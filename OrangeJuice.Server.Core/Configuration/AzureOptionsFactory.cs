namespace OrangeJuice.Server.Configuration
{
	public class AzureOptionsFactory : Factory.IFactory<AzureOptions>
	{
		#region Constants
		// TODO: support environments!
		private const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=orangejuicedev;AccountKey=Hc+ThPW3d5Sokm2mMiIG8FCaoKpuUrncJlihwy7Gzf+Pu7b0fAa9NEmQMPKr48bdUnx3uKvyqJaKWj843RrwNw==";

		private const string ProductContainer = "products";
		#endregion

		#region IFactory members
		public AzureOptions Create()
		{
			return new AzureOptions
			{
				ConnectionString = ConnectionString,
				ProductContainer = ProductContainer
			};
		}
		#endregion
	}
}