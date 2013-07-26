using System;
using System.Net;
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
			const string actual = AppKeyHeaderHandler.AppKeyHeaderName;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void ErrorCode_Should_Return_Forbidden()
		{
			// Arrange
			AppKeyHeaderHandler handler = CreateHandler();
			const HttpStatusCode expected = HttpStatusCode.Forbidden;

			// Act
			HttpStatusCode actual = handler.ErrorCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void ValidateKey_Should_Return_True_When_Headers_Contains_AppKey()
		{
			// Arrange
			Guid appKey = Guid.NewGuid();
			AppKeyHeaderHandler handler = CreateHandler(appKey);
			HttpRequestMessage request = CreateRequest(AppKeyHeaderHandler.AppKeyHeaderName, appKey);

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateKey_Should_Return_False_When_Headers_Contains_AppKey_But_It_Is_Not_Guid()
		{
			// Arrange
			AppKeyHeaderHandler handler = CreateHandler();
			HttpRequestMessage request = CreateRequest(AppKeyHeaderHandler.AppKeyHeaderName, "not-a-guid");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeFalse();
		}

		[TestMethod]
		public void ValidateKey_Should_Return_False_When_Headers_Does_Not_Contain_AppKey()
		{
			// Arrange
			AppKeyHeaderHandler handler = CreateHandler();
			HttpRequestMessage request = CreateRequest("any-name", "any-value");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeFalse();
		}

		private static AppKeyHeaderHandler CreateHandler(Guid? appKey = null)
		{
			return new AppKeyHeaderHandler(appKey ?? Guid.NewGuid());
		}

		private static HttpRequestMessage CreateRequest(string name, object value)
		{
			HttpRequestMessage request = new HttpRequestMessage();
			request.Headers.Add(name, value.ToString());
			return request;
		}
	}
}