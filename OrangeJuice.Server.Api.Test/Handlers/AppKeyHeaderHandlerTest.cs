using System;
using System.Net.Http;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Handlers;

namespace OrangeJuice.Server.Api.Test.Handlers
{
	[TestClass]
	public class AppKeyHeaderHandlerTest
	{
		[TestMethod]
		public void AppKeyHeaderName_Should_Return_XApiKey()
		{
			// Arrange
			const string expected = "X-ApiKey";

			// Act
			const string actual = HeaderAppKeyHandler.AppKeyHeaderName;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void ValidateKey_Should_Return_True_When_Headers_Contains_AppKey()
		{
			// Arrange
			Guid appKey = Guid.NewGuid();
			HeaderAppKeyHandler handler = CreateHandler(appKey);
			HttpRequestMessage request = CreateRequest(HeaderAppKeyHandler.AppKeyHeaderName, appKey);

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateKey_Should_Return_False_When_Headers_Contains_AppKey_But_It_Is_Not_Guid()
		{
			// Arrange
			HeaderAppKeyHandler handler = CreateHandler();
			HttpRequestMessage request = CreateRequest(HeaderAppKeyHandler.AppKeyHeaderName, "not-a-guid");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeFalse();
		}

		[TestMethod]
		public void ValidateKey_Should_Return_False_When_Headers_Does_Not_Contain_AppKey()
		{
			// Arrange
			HeaderAppKeyHandler handler = CreateHandler();
			HttpRequestMessage request = CreateRequest("any-name", "any-value");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeFalse();
		}

		private static HeaderAppKeyHandler CreateHandler(Guid? appKey = null)
		{
			return new HeaderAppKeyHandler(appKey ?? Guid.NewGuid());
		}

		private static HttpRequestMessage CreateRequest(string name, object value)
		{
			HttpRequestMessage request = new HttpRequestMessage();
			request.Headers.Add(name, value.ToString());
			return request;
		}
	}
}