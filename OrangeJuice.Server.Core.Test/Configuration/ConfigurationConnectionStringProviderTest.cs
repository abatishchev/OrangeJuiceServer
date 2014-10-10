using Xunit;

using Moq;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Test.Configuration
{
	public class ConfigurationConnectionStringProviderTest
	{
		[Fact]
		public void GetDefaultConnectionString_Should_Call_ConfigurationProvider_Get()
		{
			// Arrange
			var configurationProviderMock = new Mock<IConfigurationProvider>();
			configurationProviderMock.Setup(p => p.GetValue(It.IsAny<string>()));

			IConnectionStringProvider connectionStringProvider = new ConfigurationConnectionStringProvider(configurationProviderMock.Object);

			// Act
			string connectionString = connectionStringProvider.GetDefaultConnectionString();

			// Assert
			configurationProviderMock.VerifyAll();
		}

		[Fact]
		public void GetDefaultConnectionString_Should_Pass_Sql_ConnectionString_ConfigurationProvider_Get()
		{
			// Arrange
			var configurationProviderMock = new Mock<IConfigurationProvider>();
			configurationProviderMock.Setup(p => p.GetValue("sql:ConnectionString"));

			IConnectionStringProvider connectionStringProvider = new ConfigurationConnectionStringProvider(configurationProviderMock.Object);

			// Act
			string connectionString = connectionStringProvider.GetDefaultConnectionString();

			// Assert
			configurationProviderMock.VerifyAll();
		}
	}
}