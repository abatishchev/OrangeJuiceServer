using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;


using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsOptionsFactoryTest
	{
		[TestMethod]
		public void Create_Should_Return_Options_Having_All_Properties()
		{
			// Arrange
			IFactory<AwsOptions> factory = new AwsOptionsFactory();

			// Act
			AwsOptions options = factory.Create();

			// Assert
			options.AccessKey.Should().NotBeNullOrEmpty();
			options.AssociateTag.Should().NotBeNullOrEmpty();
			options.SecretKey.Should().NotBeNullOrEmpty();
		}
	}
}