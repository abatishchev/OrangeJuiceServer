using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Test.Configuration
{
	[TestClass]
	public class AppSettingsConfigurationProviderTest
	{
		[TestMethod]
		public void GetValue_Should_Return_Value_From_AppSettings()
		{
			// Arrange
			IConfigurationProvider configurationProvider = new AppSettingsConfigurationProvider();

			// Act
			string key = configurationProvider.GetValue("Test");

			// Assert
			key.Should().Be("Value");
		}

		[TestMethod]
		public void GetValue_Should_Return_Null_When_AppSettings_Does_Not_Contain_Key_Specified()
		{
			// Arrange
			IConfigurationProvider configurationProvider = new AppSettingsConfigurationProvider();

			// Act
			string actual = configurationProvider.GetValue("NotExistingKey");

			// Assert
			actual.Should().BeNull();
		}
	}
}