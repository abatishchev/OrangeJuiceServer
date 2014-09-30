using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Test.Configuration
{
	[TestClass]
	public class AzureOptionsFactoryTest
	{
		[TestMethod]
		public void Create_Should_Return_AzureOptions_Having_BlobConnectionString_Returnd_By_ConfigurationProvider_Get()
		{
			// Arrange
			const string expected = "connectionString";
			
			var providerMock = new Mock<IConfigurationProvider>();
			providerMock.Setup(p => p.GetValue("blob:ConnectionString")).Returns(expected);

			var factory = new AzureOptionsFactory(providerMock.Object);

			// Act
			string actual = factory.Create().ConnectionString;

			// Assert
			providerMock.VerifyAll();
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void Create_Should_Return_AzureOptions_Having_ProductContainerName_Returnd_By_ConfigurationProvider_Get()
		{
			// Arrange
			const string expected = "products";

			var providerMock = new Mock<IConfigurationProvider>();
			providerMock.Setup(p => p.GetValue("blob:Products")).Returns(expected);

			var factory = new AzureOptionsFactory(providerMock.Object);

			// Act
			string actual = factory.Create().ProductsContainer;

			// Assert
			providerMock.VerifyAll();
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void Create_Should_Return_AzureOptions_Having_All_Properties()
		{
			// Arrange
			var providerMock = new Mock<IConfigurationProvider>();
			providerMock.Setup(p => p.GetValue(It.IsAny<string>())).Returns<string>(s => s);

			var factory = new AzureOptionsFactory(providerMock.Object);

			// Act
			AzureOptions options = factory.Create();

			// Assert
			options.ConnectionString.Should().NotBeNullOrEmpty();
			options.ProductsContainer.Should().NotBeNullOrEmpty();
		}
	}
}