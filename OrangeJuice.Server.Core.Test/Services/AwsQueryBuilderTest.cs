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
		#region Test methods
		[TestMethod]
		public void BuildUrl_Should_Pass_Arguments_To_ArgumentBuilder()
		{
			// Arrange
			IStringDictionary args = new StringDictionary();

			var argumentBuilderMock = CreateArgumentBuilder();

			IQueryBuilder queryBuilder = CreateQueryBuilder(argumentBuilderMock.Object);

			// Act
			queryBuilder.BuildUrl(args);

			// Assert
			argumentBuilderMock.Verify(b => b.BuildArgs(args), Times.Once);
		}

		[TestMethod]
		public void BuildUrl_Should_Append_Signature()
		{
			// Arrange
			const string signature = "signature";

			IQuerySigner querySigner = CreateQuerySigner(signature);
			IQueryBuilder queryBuilder = CreateQueryBuilder(querySigner: querySigner);

			// Act
			Uri url = queryBuilder.BuildUrl(new StringDictionary());

			// Assert
			url.Query.Should().EndWith(String.Format("Signature={0}", signature));
		}

		[TestMethod]
		public void BuildUrl_Should_Call_UrlEncoder_Encode_For_Each_Argument()
		{
			// Arrange
			var urlEncoderMock = CreateUrlEncoder();

			IQueryBuilder queryBuilder = CreateQueryBuilder(urlEncoder: urlEncoderMock.Object);

			IStringDictionary args = new StringDictionary { { "key", "value" } };

			// Act
			queryBuilder.BuildUrl(args);

			// Assert
			urlEncoderMock.Verify(e => e.Encode(It.IsAny<string>()), Times.Exactly(args.Count));
		}
		#endregion

		#region Helper methods
		private static IQueryBuilder CreateQueryBuilder(IArgumentBuilder argumentBuilder = null, IQuerySigner querySigner = null, IUrlEncoder urlEncoder = null)
		{
			return new AwsQueryBuilder(
				argumentBuilder ?? CreateArgumentBuilder().Object,
				querySigner ?? CreateQuerySigner(),
				urlEncoder ?? CreateUrlEncoder().Object);
		}

		private static Mock<IArgumentBuilder> CreateArgumentBuilder(IStringDictionary args = null)
		{
			var builderMock = new Mock<IArgumentBuilder>();
			builderMock.Setup(b => b.BuildArgs(It.IsAny<IStringDictionary>())).Returns(args ?? new StringDictionary());
			return builderMock;
		}

		private static IQuerySigner CreateQuerySigner(string signature = null)
		{
			var builderMock = new Mock<IQuerySigner>();
			builderMock.Setup(b => b.SignQuery(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
					   .Returns<string, string, string>((h, p, s) => signature ?? s);
			return builderMock.Object;
		}

		private static Mock<IUrlEncoder> CreateUrlEncoder()
		{
			var encoderMock = new Mock<IUrlEncoder>();
			encoderMock.Setup(e => e.Encode(It.IsAny<string>())).Returns<string>(s => s);
			return encoderMock;
		}
		#endregion
	}
}