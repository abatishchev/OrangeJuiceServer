using System;

using FluentAssertions;

using Xunit;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Test.Configuration
{
	public class AwsOptionsFactoryTest
	{
		[Fact]
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
			options.RequestLimit.Should().NotBe(TimeSpan.Zero);
		}
	}
}