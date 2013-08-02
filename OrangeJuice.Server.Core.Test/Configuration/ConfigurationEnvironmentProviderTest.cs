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
		public void GetCurrentEnvironment_Should_Call_ConfigurationProvider_GetValue_With_KeyName()
		{
			// Arrange
			const string environment = "SomeEnvironment";
			var configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
			configurationProviderMock.Setup(p => p.GetValue(ConfigurationEnvironmentProvider.KeyName))
									 .Returns(environment);
			IEnvironmentProvider environmentProvider = new ConfigurationEnvironmentProvider(configurationProviderMock.Object);

			// Act
			environmentProvider.GetCurrentEnvironment();

			// Assert
			configurationProviderMock.Verify(p => p.GetValue(ConfigurationEnvironmentProvider.KeyName));
		}

		[TestMethod]
		public void GetCurrentEnvironment_Should_Return_Value_Returned_By_ConfigurationProvider_GetValue()
		{
			// Arrange
			const string expected = "SomeEnvironment";
			var configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
			configurationProviderMock.Setup(p => p.GetValue(ConfigurationEnvironmentProvider.KeyName))
									 .Returns(expected);
			IEnvironmentProvider environmentProvider = new ConfigurationEnvironmentProvider(configurationProviderMock.Object);

			// Act
			string actual = environmentProvider.GetCurrentEnvironment();

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void GetCurrentEnvironment_Should_Throw_Exception_When_Environment_Is_Null()
		{
			// Arrange
			var configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
			configurationProviderMock.Setup(p => p.GetValue(ConfigurationEnvironmentProvider.KeyName))
									 .Returns((string)null);
			IEnvironmentProvider environmentProvider = new ConfigurationEnvironmentProvider(configurationProviderMock.Object);

			// Act
			Action action = () => environmentProvider.GetCurrentEnvironment();

			// Assert
			action.ShouldThrow<InvalidOperationException>();
		}

		[TestMethod]
		public void GetCurrentEnvironment_Should_Throw_Exception_When_Environment_Is_Empty()
		{
			// Arrange
			var configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
			configurationProviderMock.Setup(p => p.GetValue(ConfigurationEnvironmentProvider.KeyName))
									 .Returns("");
			IEnvironmentProvider environmentProvider = new ConfigurationEnvironmentProvider(configurationProviderMock.Object);

			// Act
			Action action = () => environmentProvider.GetCurrentEnvironment();

			// Assert
			action.ShouldThrow<InvalidOperationException>();
		}
	}
}