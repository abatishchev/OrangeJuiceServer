using System;

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
		public void Ctor_Should_Throw_Exception_When_ConfigurationProvider_Is_Null()
		{
			// Arrange
			const IConfigurationProvider configurationProvider = null;

			// Act
			Action action = () => new ConfigurationEnvironmentProvider(configurationProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("configurationProvider");
		}

		[TestMethod]
		public void GetCurrentEnvironment_Should_Call_ConfigurationProvider_GetValue()
		{
			// Arrange
			var configurationProviderMock = new Mock<IConfigurationProvider>();
			IEnvironmentProvider environmentProvider = new ConfigurationEnvironmentProvider(configurationProviderMock.Object);

			// Act
			environmentProvider.GetCurrentEnvironment();

			// Assert
			configurationProviderMock.Verify(p => p.GetValue(ConfigurationEnvironmentProvider.KeyName));
		}
	}
}