using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Test.Configuration
{
	[TestClass]
	public class AwsOptionsFactoryTest
	{
		[TestMethod]
		public void Create_Should_Return_AwsOptions_Having_All_Properties()
		{
			// Arrange
			var factory = new AwsOptionsFactory();

			// Act
			AwsOptions options = factory.Create();

			// Assert
			options.AccessKey.Should().NotBeNullOrEmpty();
			options.AssociateTag.Should().NotBeNullOrEmpty();
			options.SecretKey.Should().NotBeNullOrEmpty();
		}
	}
}