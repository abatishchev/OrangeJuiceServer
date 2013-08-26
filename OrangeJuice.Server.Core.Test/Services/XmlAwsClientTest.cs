using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Builders;
using OrangeJuice.Server.Services;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class XmlAwsClientTest
	{
		#region Test methods
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_ArgumentBuilder_Is_Null()
		{
			// Arrange
			const ArgumentBuilder argumentBuilder = null;
			const QueryBuilder queryBuilder = null;
			const SignatureBuilder signatureBuilder = null;
			const HttpDocumentLoader documentLoader = null;

			// Act
			Action action = () => new XmlAwsClient(argumentBuilder, queryBuilder, signatureBuilder, documentLoader);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("argumentBuilder");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_QueryBuilder_Is_Null()
		{
			// Arrange
			ArgumentBuilder argumentBuilder = CreateArgumentBuilder();
			const QueryBuilder queryBuilder = null;
			const SignatureBuilder signatureBuilder = null;
			const HttpDocumentLoader documentLoader = null;

			// Act
			Action action = () => new XmlAwsClient(argumentBuilder, queryBuilder, signatureBuilder, documentLoader);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("queryBuilder");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_SignatureBuilder_Is_Null()
		{
			// Arrange
			ArgumentBuilder argumentBuilder = CreateArgumentBuilder();
			QueryBuilder queryBuilder = CreateQueryBuilder();
			SignatureBuilder signatureBuilder = null;
			const HttpDocumentLoader documentLoader = null;

			// Act
			Action action = () => new XmlAwsClient(argumentBuilder, queryBuilder, signatureBuilder, documentLoader);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("signatureBuilder");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_DocumentLoader_Is_Null()
		{
			// Arrange
			ArgumentBuilder argumentBuilder = CreateArgumentBuilder();
			QueryBuilder queryBuilder = CreateQueryBuilder();
			SignatureBuilder signatureBuilder = CreateSignatureBuilder();
			const IDocumentLoader documentLoader = null;

			// Act
			Action action = () => new XmlAwsClient(argumentBuilder, queryBuilder, signatureBuilder, documentLoader);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("documentLoader");
		}

		[TestMethod]
		public void SearchItem_Should_()
		{
			// Arrange

			// Act

			// Assert
			Assert.Inconclusive();
		}

		[TestMethod]
		public void LookupAttributes_Should_()
		{
			// Arrange

			// Act

			// Assert
			Assert.Inconclusive();
		}

		[TestMethod]
		public void LookupImages_Should_()
		{
			// Arrange

			// Act

			// Assert
			Assert.Inconclusive();
		}
		#endregion

		#region Helper methods
		private static ArgumentBuilder CreateArgumentBuilder()
		{
			return new ArgumentBuilder("anyTag");
		}

		private static QueryBuilder CreateQueryBuilder()
		{
			return new QueryBuilder(
				"anyKey",
				new Mock<IUrlEncoder>().Object,
				new Mock<IDateTimeProvider>().Object);
		}

		private static SignatureBuilder CreateSignatureBuilder()
		{
			return new SignatureBuilder(
				new Mock<System.Security.Cryptography.HashAlgorithm>().Object,
				new Mock<IUrlEncoder>().Object);
		}
		#endregion
	}
}