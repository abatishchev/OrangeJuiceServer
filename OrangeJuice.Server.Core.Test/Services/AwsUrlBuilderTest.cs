using System;
using System.Collections.Specialized;
using System.Net.Http;

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
	public class AwsUrlBuilderTest
	{
		#region Test methods
		[TestMethod]
		public void BuildUrl_Should_Pass_Arguments_To_ArgumentBuilder()
		{
			// Arrange
			IStringDictionary args = new StringDictionary();

			var argumentBuilderMock = new Mock<IArgumentBuilder>();
			argumentBuilderMock.Setup(b => b.BuildArgs(It.IsAny<IStringDictionary>())).Returns(args);

			IUrlBuilder urlBuilder = CreateUrlBuilder(argumentBuilderMock.Object);

			// Act
			urlBuilder.BuildUrl(args);

			// Assert
			argumentBuilderMock.Verify(b => b.BuildArgs(args), Times.Once);
		}

		[TestMethod]
		public void BuildUrl_Should_Return_Url_Having_Arguments()
		{
			// Arrange
			const string key = "key";
			const string value = "value";

			IUrlBuilder urlBuilder = CreateUrlBuilder();

			// Act
			Uri url = urlBuilder.BuildUrl(new StringDictionary { { key, value } });

			// Assert
			NameValueCollection collection = url.ParseQueryString();
			collection[key].Should().Be(value);
		}

		[TestMethod]
		public void BuildUrl_Should_Return_Url_Having_Signature()
		{
			// Arrange
			const string signature = "signature";

			IQuerySigner querySigner = CreateQuerySigner(signature);
			IUrlBuilder urlBuilder = CreateUrlBuilder(querySigner: querySigner);

			// Act
			Uri url = urlBuilder.BuildUrl(new StringDictionary());

			// Assert
			url.Query.Should().EndWith(String.Format("Signature={0}", signature));
		}
		#endregion

		#region Helper methods
		private static IUrlBuilder CreateUrlBuilder(IArgumentBuilder argumentBuilder = null, IQueryBuilder queryBuilder = null, IQuerySigner querySigner = null)
		{
			return new AwsUrlBuilder(
				argumentBuilder ?? new AwsArgumentBuilder("", "", new UtcDateTimeProvider()),
				queryBuilder ?? new EncodedQueryBuilder(new PercentUrlEncoder(new PercentUrlEncodingPipeline())),
				querySigner ?? CreateQuerySigner());
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