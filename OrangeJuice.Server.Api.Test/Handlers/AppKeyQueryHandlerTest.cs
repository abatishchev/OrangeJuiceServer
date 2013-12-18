using System;
using System.Net.Http;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Handlers;

namespace OrangeJuice.Server.Api.Test.Handlers
{
	[TestClass]
	public class AppKeyQueryHandlerTest
	{
		[TestMethod]
		public void AppKeySegmentName_Should_Return_ApiKey()
		{
			// Arrange
			const string expected = "appKey";

			// Act
			const string actual = QueryAppKeyHandler.AppKeySegmentName;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void ValidateKey_Should_Return_True_When_Query_Contains_AppKey()
		{
			// Arrange
			Guid appKey = Guid.NewGuid();
			QueryAppKeyHandler handler = CreateHandler(appKey);
			HttpRequestMessage request = CreateRequest(QueryAppKeyHandler.AppKeySegmentName, appKey);

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateKey_Should_Return_False_When_Query_Contains_AppKey_But_It_Is_Not_Guid()
		{
			// Arrange
			QueryAppKeyHandler handler = CreateHandler();
			HttpRequestMessage request = CreateRequest(QueryAppKeyHandler.AppKeySegmentName, "not-a-guid");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeFalse();
		}

		[TestMethod]
		public void ValidateKey_Should_Return_False_When_Query_Does_Not_Contain_AppKey()
		{
			// Arrange
			QueryAppKeyHandler handler = CreateHandler();
			HttpRequestMessage request = CreateRequest("any-name", "any-value");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeFalse();
		}

		private static QueryAppKeyHandler CreateHandler(Guid? appKey = null)
		{
			return new QueryAppKeyHandler(appKey ?? Guid.NewGuid());
		}

		private static HttpRequestMessage CreateRequest(string name, object value)
		{
			return new HttpRequestMessage
			{
				RequestUri = new Uri(
					new Uri("http://example.com"),
					new Uri(String.Format("?{0}={1}", name, value), UriKind.Relative))
			};
		}
	}
}