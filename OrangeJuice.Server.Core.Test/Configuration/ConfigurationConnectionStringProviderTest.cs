using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Test.Configuration
{
	[TestClass]
	public class ConfigurationConnectionStringProviderTest
	{
		[TestMethod]
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

		[TestMethod]
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