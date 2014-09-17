using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Test.Configuration
{
	[TestClass]
	public class AzureOptionsFactoryTest
	{
		[TestMethod]
		public void Create_Should_Return_AzureOptions_Having_All_Properties()
		{
			// Arrange
			var factory = new AzureOptionsFactory();

			// Act
			AzureOptions options = factory.Create();

			// Assert
			options.ConnectionString.Should().NotBeNullOrEmpty();
			options.ProductContainer.Should().NotBeNullOrEmpty();
		}
	}
}