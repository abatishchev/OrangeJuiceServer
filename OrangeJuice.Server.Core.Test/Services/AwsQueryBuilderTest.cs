using System;
using System.Collections.Specialized;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;

using IStringDictionary = System.Collections.Generic.IDictionary<string, string>;
using StringDictionary = System.Collections.Generic.Dictionary<string, string>;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsQueryBuilderTest
	{
		#region Test methods
		[TestMethod]
		public void BuildUrl_Should_Pass_Arguments_To_ArgumentBuilder()
		{
			// Arrange
			IStringDictionary args = new StringDictionary();

			var builderMock = new Mock<IArgumentBuilder>();
			builderMock.Setup(b => b.BuildArgs(args)).Returns(new NameValueCollection());

			IQueryBuilder queryBuilder = CreateQueryBuilder(builderMock.Object);

			// Act
			queryBuilder.BuildUrl(args);

			// Assert
			builderMock.Verify(b => b.BuildArgs(args), Times.Once);
		}

		[TestMethod]
		public void BuildUrl_Should_Append_Signature()
		{
			// Arrange
			const string signature = "signature";
			var querySigner = CreateQuerySigner(signature);
			IQueryBuilder queryBuilder = CreateQueryBuilder(querySigner: querySigner);

			// Act
			string signedQuery = queryBuilder.BuildUrl(new StringDictionary());

			// Assert
			signedQuery.Should().EndWith(String.Format("Signature={0}", signature));
		}
		#endregion

		#region Helper methods
		private static IQueryBuilder CreateQueryBuilder(IArgumentBuilder argumentBuilder = null, IQuerySigner querySigner = null)
		{
			return new AwsQueryBuilder(
				argumentBuilder ?? CreateArgumentBuilder(),
				querySigner ?? CreateQuerySigner());
		}

		private static IArgumentBuilder CreateArgumentBuilder()
		{
			var builderMock = new Mock<IArgumentBuilder>();
			builderMock.Setup(b => b.BuildArgs(It.IsAny<IStringDictionary>())).Returns(new NameValueCollection());
			return builderMock.Object;
		}

		private static IQuerySigner CreateQuerySigner(string signature = null)
		{
			var builderMock = new Mock<IQuerySigner>();
			builderMock.Setup(b => b.SignQuery(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
					   .Returns<string, string, string>((h, p, s) => signature ?? s);
			return builderMock.Object;
		}
		#endregion
	}
}