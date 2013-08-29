using System;
using System.Security.Cryptography;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Builders;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Test.Builders
{
	[TestClass]
	public class SignatureBuilderTest
	{
		[TestMethod]
		// ReSharper disable once InconsistentNaming
		public void CreateHashAlgorithm_Should_Return_HMACSHA256()
		{
			// Arrange
			const string secretKey = "anyKey";

			// Act
			HashAlgorithm hashAlgorithm = SignatureBuilder.CreateHashAlgorithm(secretKey);

			// Assert
			hashAlgorithm.Should().BeOfType<HMACSHA256>();
		}

		[TestMethod]
		public void SignQuery_Should_Prepend_ServiceUrl()
		{
			// Arrange
			const string query = "a=1&b=2";
			var signatureBuilder = CreateSignatureBuilder();

			// Act
			string signedQuery = signatureBuilder.SignQuery(query);

			// Assert
			signedQuery.Should().StartWith(String.Format("{0}://{1}{2}", Uri.UriSchemeHttp, SignatureBuilder.RequestEndpoint, SignatureBuilder.RequestPath));
		}

		[TestMethod]
		public void SignQuery_Should_Append_Signature()
		{
			// Arrange
			const string query = "a=1&b=2";
			var signatureBuilder = CreateSignatureBuilder();
			string signature = signatureBuilder.CreateSignature(query);

			// Act
			string signedQuery = signatureBuilder.SignQuery(query);

			// Assert
			signedQuery.Should().EndWith(String.Format("&Signature={0}", signature));
		}

		[TestMethod]
		public void SignQuery_Should_Call_UrlEncoder_Encode()
		{
			// Arrange
			const string query = "a=1&b=2";
			var urlEncoderMock = CreateUrlEncoder();
			var signatureBuilder = CreateSignatureBuilder(urlEncoderMock.Object);

			// Act
			signatureBuilder.SignQuery(query);

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
			string signature = signatureBuilder.CreateSignature(query);

			// Act
			signatureBuilder.SignQuery(query);

			// Assert
			urlEncoderMock.Verify(e => e.Encode(signature));
		}

		private static SignatureBuilder CreateSignatureBuilder(IUrlEncoder urlEncoder = null)
		{
			return new SignatureBuilder("anyKey", urlEncoder ?? CreateUrlEncoder().Object);
		}

		private static Mock<IUrlEncoder> CreateUrlEncoder()
		{
			var urlEncoderMock = new Mock<IUrlEncoder>();
			urlEncoderMock.Setup(e => e.Encode(It.IsAny<string>())).Returns<string>(s => s);
			return urlEncoderMock;
		}
	}
}