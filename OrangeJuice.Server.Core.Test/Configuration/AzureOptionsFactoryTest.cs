using System;

using Factory;
using FluentAssertions;
using Moq;

using OrangeJuice.Server.Configuration;

using Xunit;

namespace OrangeJuice.Server.Test.Configuration
{
	public class AzureOptionsFactoryTest
	{
		#region Tests
		[Theory]
		[InlineData(typeof(AzureOptionsFactory))]
		[InlineData(typeof(FSharp.Configuration.AzureOptionsFactory))]
		public void Create_Should_Return_AzureOptions_Having_BlobConnectionString_Returnd_By_ConfigurationProvider_Get(Type type)
		{
			// Arrange
			const string expected = "connectionString";

			var providerMock = new Mock<IConfigurationProvider>();
			providerMock.Setup(p => p.GetValue("blob:ConnectionString")).Returns(expected);

			var factory = CreateFactory(type, providerMock.Object);

			// Act
			string actual = factory.Create().ConnectionString;

			// Assert
			providerMock.VerifyAll();
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData(typeof(AzureOptionsFactory))]
		[InlineData(typeof(FSharp.Configuration.AzureOptionsFactory))]
		public void Create_Should_Return_AzureOptions_Having_ProductContainerName_Returnd_By_ConfigurationProvider_Get(Type type)
		{
			// Arrange
			const string expected = "products";

			var providerMock = new Mock<IConfigurationProvider>();
			providerMock.Setup(p => p.GetValue("blob:Products")).Returns(expected);

			var factory = CreateFactory(type, providerMock.Object);

			// Act
			string actual = factory.Create().ProductsContainer;

			// Assert
			providerMock.VerifyAll();
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData(typeof(AzureOptionsFactory))]
		[InlineData(typeof(FSharp.Configuration.AzureOptionsFactory))]
		public void Create_Should_Return_AzureOptions_Having_All_Properties(Type type)
		{
			// Arrange
			var providerMock = new Mock<IConfigurationProvider>();
			providerMock.Setup(p => p.GetValue(It.IsAny<string>())).Returns<string>(s => s);

			var factory = CreateFactory(type, providerMock.Object);

			// Act
			AzureOptions options = factory.Create();

			// Assert
			options.Should().NotBeNull();
			options.ConnectionString.Should().NotBeNullOrEmpty();
			options.ProductsContainer.Should().NotBeNullOrEmpty();
		}
		#endregion

		#region Helpers methods
		private static IFactory<AzureOptions> CreateFactory(Type type, IConfigurationProvider configurationProvider)
		{
			return (IFactory<AzureOptions>)Activator.CreateInstance(type, configurationProvider);
		}
		#endregion
	}
}