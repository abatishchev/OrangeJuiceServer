using System.Security.Cryptography;

using FluentAssertions;

using Xunit;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	public class AwsAlgorithmFactoryTest
	{
		[Fact]
		// ReSharper disable once InconsistentNaming
		public void CreateHashAlgorithm_Should_Return_HMACSHA256()
		{
			// Arrange
			var factory = new AwsAlgorithmFactory(new AwsOptions { SecretKey = "Key" });

			// Act
			HashAlgorithm hashAlgorithm = factory.Create();

			// Assert
			hashAlgorithm.Should().BeOfType<HMACSHA256>();
		}
	}
}