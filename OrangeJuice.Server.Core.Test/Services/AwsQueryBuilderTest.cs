using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;
using OrangeJuice.Server.Web;

using IStringDictionary = System.Collections.Generic.IDictionary<string, string>;
using StringDictionary = System.Collections.Generic.Dictionary<string, string>;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsQueryBuilderTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_ArgumentBuilder_Is_Null()
		{
			// Arrange
			const IArgumentBuilder argumentBuilder = null;
			const IArgumentFormatter argumentFormatter = null;
			const IQuerySigner signatureBuilder = null;

			// Act
			Action action = () => new AwsQueryBuilder(argumentBuilder, argumentFormatter, signatureBuilder);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("argumentBuilder");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_ArgumentFormatter_Is_Null()
		{
			// Arrange
			IArgumentBuilder argumentBuilder = CreateArgumentBuilder();
			const IArgumentFormatter argumentFormatter = null;
			const IQuerySigner signatureBuilder = null;

			// Act
			Action action = () => new AwsQueryBuilder(argumentBuilder, argumentFormatter, signatureBuilder);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("argumentFormatter");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_QuerySigner_Is_Null()
		{
			// Arrange
			IArgumentBuilder argumentBuilder = CreateArgumentBuilder();
			IArgumentFormatter argumentFormatter = CreateArgumentFormatter();
			const IQuerySigner querySigner = null;

			// Act
			Action action = () => new AwsQueryBuilder(argumentBuilder, argumentFormatter, querySigner);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("querySigner");
		}
		#endregion

		#region BuildUrl
		[TestMethod]
		public void BuildUrl_Should_Pass_Arguments_To_ArgumentBuilder()
		{
			// Arrange
			IStringDictionary args = new StringDictionary();

			var agumentBuilderMock = new Mock<IArgumentBuilder>();

			IQueryBuilder queryBuilder = CreateQueryBuilder(agumentBuilderMock.Object);

			// Act
			queryBuilder.BuildUrl(args);

			// Assert
			agumentBuilderMock.Verify(b => b.BuildArgs(args), Times.Once());
		}

		[TestMethod]
		public void BuildUrl_Should_Pass_Arguments_Returned_By_ArgumentBuilder_To_ArgumentFormatter_FormatArgs()
		{
			Assert.Inconclusive("TODO");
		}

		[TestMethod]
		public void BuildUrl_Should_Pass_Quqery_Returned_By_ArgumentFormatter_To_SignatureBuilder_SignQuery()
		{
			Assert.Inconclusive("TODO");
		}
		#endregion

		#region Helper methods
		private static IQueryBuilder CreateQueryBuilder(IArgumentBuilder argumentBuilder = null, IArgumentFormatter argumentFormatter = null, IQuerySigner querySigner = null)
		{
			return new AwsQueryBuilder(
				argumentBuilder ?? CreateArgumentBuilder(),
				argumentFormatter ?? CreateArgumentFormatter(),
				querySigner ?? CreateSignatureBuilder());
		}

		private static IArgumentBuilder CreateArgumentBuilder()
		{
			var builderMock = new Mock<IArgumentBuilder>();
			builderMock.Setup(b => b.BuildArgs(It.IsAny<IStringDictionary>())).Returns(new StringDictionary());
			return builderMock.Object;
		}

		private static IArgumentFormatter CreateArgumentFormatter()
		{
			var builderMock = new Mock<IArgumentFormatter>();
			builderMock.Setup(b => b.FormatArgs(It.IsAny<IStringDictionary>())).Returns("query");
			return builderMock.Object;
		}

		private static IQuerySigner CreateSignatureBuilder()
		{
			var builderMock = new Mock<IQuerySigner>();
			builderMock.Setup(b => b.SignQuery(It.IsAny<string>())).Returns<string>(s => s);
			return builderMock.Object;
		}
		#endregion
	}
}