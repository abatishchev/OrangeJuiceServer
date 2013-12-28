﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

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
		#region BuildUrl
		[TestMethod]
		public void BuildUrl_Should_Pass_Arguments_To_ArgumentBuilder()
		{
			// Arrange
			var builderMock = new Mock<IArgumentBuilder>();
			IQueryBuilder queryBuilder = CreateQueryBuilder(builderMock.Object);
			IStringDictionary args = new StringDictionary();

			// Act
			queryBuilder.BuildUrl(args);

			// Assert
			builderMock.Verify(b => b.BuildArgs(args), Times.Once());
		}

		[TestMethod]
		public void BuildUrl_Should_Pass_Arguments_Returned_By_ArgumentBuilder_To_ArgumentFormatter_FormatArgs()
		{
			// Arrange
			IStringDictionary args = new StringDictionary();

			var builderMock = new Mock<IArgumentBuilder>();
			builderMock.Setup(b => b.BuildArgs(It.IsAny<IStringDictionary>())).Returns(args);

			var formatterMock = new Mock<IArgumentFormatter>();

			IQueryBuilder queryBuilder = CreateQueryBuilder(builderMock.Object, formatterMock.Object);

			// Act
			queryBuilder.BuildUrl(args);

			// Assert
			formatterMock.Verify(f => f.FormatArgs(args), Times.Once());
		}

		[TestMethod]
		public void BuildUrl_Should_Pass_Query_Returned_By_ArgumentFormatter_To_QuerySigner_SignQuery()
		{
			// Arrange
			var signerMock = new Mock<IQuerySigner>();

			const string query = "query";
			var formatterMock = new Mock<IArgumentFormatter>();
			formatterMock.Setup(f => f.FormatArgs(It.IsAny<IStringDictionary>())).Returns(query);

			IQueryBuilder queryBuilder = CreateQueryBuilder(argumentFormatter: formatterMock.Object, querySigner: signerMock.Object);
			IStringDictionary args = new StringDictionary();

			// Act
			queryBuilder.BuildUrl(args);

			// Assert
			signerMock.Verify(b => b.SignQuery(query), Times.Once());
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