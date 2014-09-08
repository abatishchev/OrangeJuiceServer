using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class AwsUrlBuilderTest
	{
		#region Test methods
		[TestMethod]
		public void BuildUrl_Should_Pass_SearchCriteria_To_ArgumentBuilder()
		{
			// Arrange
			ProductDescriptorSearchCriteria searchCriteria = new ProductDescriptorSearchCriteria();

			var argumentBuilderMock = new Mock<IArgumentBuilder>();
			argumentBuilderMock.Setup(b => b.BuildArgs(searchCriteria)).Returns(new Dictionary<string, string>());

			IUrlBuilder urlBuilder = CreateUrlBuilder(argumentBuilderMock.Object);

			// Act
			urlBuilder.BuildUrl(searchCriteria);

			// Assert
			argumentBuilderMock.VerifyAll();
		}

		[TestMethod]
		public void BuildUrl_Should_Return_Url_Having_Signature()
		{
			// Arrange
			const string signature = "signature";

			IQuerySigner querySigner = CreateQuerySigner(signature);
			IUrlBuilder urlBuilder = CreateUrlBuilder(querySigner: querySigner);

			// Act
			Uri url = urlBuilder.BuildUrl(new ProductDescriptorSearchCriteria());

			// Assert
			url.Query.Should().EndWith(String.Format("Signature={0}", signature));
		}
		#endregion

		#region Helper methods
		private static IUrlBuilder CreateUrlBuilder(IArgumentBuilder argumentBuilder = null, IQueryBuilder queryBuilder = null, IQuerySigner querySigner = null)
		{
			return new AwsUrlBuilder(
				argumentBuilder ?? new AwsArgumentBuilder(new AwsOptions { AccessKey = "", AssociateTag = "", SecretKey = "" }, new UtcDateTimeProvider()),
				queryBuilder ?? new EncodedQueryBuilder(new PercentUrlEncoder(new PercentUrlEncodingPipeline())),
				querySigner ?? CreateQuerySigner());
		}

		private static IQuerySigner CreateQuerySigner(string signature = null)
		{
			var builderMock = new Mock<IQuerySigner>();
			builderMock.Setup(b => b.CreateSignature(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				   .Returns<string, string, string>((h, p, s) => signature ?? Guid.NewGuid().ToString());
			return builderMock.Object;
		}
		#endregion
	}
}