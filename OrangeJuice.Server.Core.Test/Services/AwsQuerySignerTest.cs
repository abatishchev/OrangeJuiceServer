using System.Security.Cryptography;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;


using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsQuerySignerTest
	{
		#region Test methods
		[TestMethod]
		// ReSharper disable once InconsistentNaming
		public void CreateHashAlgorithm_Should_Return_HMACSHA256()
		{
			// Arrange
			const string secretKey = "anyKey";

			// Act
			HashAlgorithm hashAlgorithm = AwsQuerySigner.CreateHashAlgorithm(secretKey);

			// Assert
			hashAlgorithm.Should().BeOfType<HMACSHA256>();
		}
		#endregion
	}
}