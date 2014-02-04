using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Test.Configuration
{
	[TestClass]
	public class ConfigurationEnvironmentProviderTest
	{
		[TestMethod]
		public void GetCurrentEnvironment_Should_Pass_KeyName_Environment_To_ConfigurationProvider_GetValue()
		{
			// Arrange
			const string environment = Environment.Testing;

			var configurationProviderMock = new Mock<IConfigurationProvider>();
			configurationProviderMock.Setup(p => p.GetValue("Environment")).Returns(environment);

			IEnvironmentProvider environmentProvider = new ConfigurationEnvironmentProvider(configurationProviderMock.Object);

			// Act
			environmentProvider.GetCurrentEnvironment();

			// Assert
			configurationProviderMock.Verify(p => p.GetValue("Environment"));
		}

		[TestMethod]
		public void GetCurrentEnvironment_Should_Return_Value_Returned_By_ConfigurationProvider_GetValue()
		{
			// Arrange
			const string expected = Environment.Testing;

			var configurationProviderMock = new Mock<IConfigurationProvider>();
			configurationProviderMock.Setup(p => p.GetValue("Environment")).Returns(expected);

			IEnvironmentProvider environmentProvider = new ConfigurationEnvironmentProvider(configurationProviderMock.Object);

			// Act
			string actual = environmentProvider.GetCurrentEnvironment();

			// Assert
			actual.Should().Be(expected);
		}
	}
}