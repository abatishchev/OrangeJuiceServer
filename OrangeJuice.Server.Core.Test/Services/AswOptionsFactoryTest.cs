using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AswOptionsFactoryTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_ConfigurationProvider_Is_Null()
		{
			// Arrange
			const IConfigurationProvider configurationProvider = null;

			// Act
			Action action = () => new AswOptionsFactory(configurationProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("configurationProvider");
		}

		[TestMethod]
		public void Create_Should_Return_Options_With_Populated_Properties()
		{
			// Arrange
			var configurationProviderMock = new Mock<IConfigurationProvider>(MockBehavior.Strict);
			configurationProviderMock.Setup(p => p.GetValue(It.IsAny<string>())).Returns("non-empty-string");
			var factory = new AswOptionsFactory(configurationProviderMock.Object);

			// Act
			AwsOptions options = factory.Create();

			// Assert
			options.AccessKey.Should().NotBeNullOrEmpty();
			options.AssociateTag.Should().NotBeNullOrEmpty();
			options.SecretKey.Should().NotBeNullOrEmpty();
		}

		[TestMethod]
		public void Create_Should_Call_ConfigurationProvider_GetValue()
		{
			// Arrange
			var configurationProviderMock = new Mock<IConfigurationProvider>();
			var factory = new AswOptionsFactory(configurationProviderMock.Object);

			// Act
			factory.Create();

			// Assert
			configurationProviderMock.Verify(p => p.GetValue(It.IsAny<string>()), Times.Exactly(3));
		}

		[TestMethod]
		public void Create_Should_Pass_Key_To_ConfigurationProvider_GetValue()
		{
			// Arrange
			var configurationProviderMock = new Mock<IConfigurationProvider>();
			var factory = new AswOptionsFactory(configurationProviderMock.Object);

			// Act
			factory.Create();

			// Assert
			configurationProviderMock.Verify(p => p.GetValue(AswOptionsFactory.AwsAccessKey));
			configurationProviderMock.Verify(p => p.GetValue(AswOptionsFactory.AwsAssociateTag));
			configurationProviderMock.Verify(p => p.GetValue(AswOptionsFactory.AwsSecretKey));
		}
	}
}