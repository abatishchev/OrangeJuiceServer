using System.Security.Cryptography;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;
using OrangeJuice.Server.Web;

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

		[TestMethod]
		public void SignQuery_Should_Call_UrlEncoder_Encode()
		{
			// Arrange
			const string query = "a=1&b=2";

			var urlEncoderMock = CreateUrlEncoder();
			var signatureBuilder = CreateSignatureBuilder(urlEncoderMock.Object);

			// Act
			signatureBuilder.SignQuery("host", "path", query);

			// Assert
			urlEncoderMock.Verify(e => e.Encode(It.IsAny<string>()), Times.Once);
		}

		[TestMethod]
		public void SignQuery_Should_Pass_Signature_To_UrlEncoder_Encode()
		{
			// Arrange
			const string query = "a=1&b=2";

			var urlEncoderMock = CreateUrlEncoder();
			var signatureBuilder = CreateSignatureBuilder(urlEncoderMock.Object);

			// Act
			signatureBuilder.SignQuery("host", "path", query);

			// Assert
			urlEncoderMock.Verify(e => e.Encode(It.IsAny<string>()), Times.Once);
		}
		#endregion

		#region Helper methods
		private static AwsQuerySigner CreateSignatureBuilder(IUrlEncoder urlEncoder = null)
		{
			return new AwsQuerySigner("anyKey", urlEncoder ?? CreateUrlEncoder().Object);
		}

		private static Mock<IUrlEncoder> CreateUrlEncoder()
		{
			var urlEncoderMock = new Mock<IUrlEncoder>();
			urlEncoderMock.Setup(e => e.Encode(It.IsAny<string>())).Returns<string>(s => s);
			return urlEncoderMock;
		}
		#endregion
	}
}