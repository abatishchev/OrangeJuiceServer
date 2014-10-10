﻿using FluentAssertions;

using Xunit;

using Moq;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Test.Configuration
{
	public class ConfigurationEnvironmentProviderTest
	{
		[Fact]
		public void GetCurrentEnvironment_Should_Pass_KeyName_EnvironmentName_To_ConfigurationProvider_GetValue()
		{
			// Arrange
			const string environment = EnvironmentName.Testing;

			var configurationProviderMock = new Mock<IConfigurationProvider>();
			configurationProviderMock.Setup(p => p.GetValue("environment:Name")).Returns(environment);

			IEnvironmentProvider environmentProvider = new ConfigurationEnvironmentProvider(configurationProviderMock.Object);

			// Act
			environmentProvider.GetCurrentEnvironment();

			// Assert
			configurationProviderMock.VerifyAll();
		}

		[Fact]
		public void GetCurrentEnvironment_Should_Return_Value_Returned_By_ConfigurationProvider_GetValue()
		{
			// Arrange
			const string expected = EnvironmentName.Testing;

			var configurationProviderMock = new Mock<IConfigurationProvider>();
			configurationProviderMock.Setup(p => p.GetValue(It.IsAny<string>())).Returns(expected);

			IEnvironmentProvider environmentProvider = new ConfigurationEnvironmentProvider(configurationProviderMock.Object);

			// Act
			string actual = environmentProvider.GetCurrentEnvironment();

			// Assert
			actual.Should().Be(expected);
		}
	}
}