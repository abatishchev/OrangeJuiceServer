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
		public void Create_Should_Return_AzureOptions_Having_Properties_Returnd_By_ConfigurationProvider_Get(Type type)
		{
			// Arrange
			const string connectionString = "connectionString";
			const string productsContainer = "products";
			const string awsOptionsContainer = "awsOptions";

			var providerMock = new Mock<IConfigurationProvider>();
			providerMock.Setup(p => p.GetValue("azure:ConnectionString")).Returns(connectionString);
			providerMock.Setup(p => p.GetValue("azure:Products")).Returns(productsContainer);
			providerMock.Setup(p => p.GetValue("azure:AwsOptions")).Returns(awsOptionsContainer);

			var factory = CreateFactory(type, providerMock.Object);

			// Act
			var actual = factory.Create();

			// Assert
			providerMock.VerifyAll();
			actual.ConnectionString.Should().Be(connectionString);
			actual.ProductsContainer.Should().Be(productsContainer);
			actual.AwsOptionsContainer.Should().Be(awsOptionsContainer);
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