using System;
using System.Net.Http;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Handlers;

namespace OrangeJuice.Server.Api.Test.Handlers
{
	[TestClass]
	public class HeaderAppVersionHandlerTest
	{
		[TestMethod]
		public void ValidateKey_Should_Return_True_When_Headers_Contains_AppVersion()
		{
			// Arrange
			Version appVersion = new Version();
			HeaderAppVersionHandler handler = CreateHandler(appVersion);
			HttpRequestMessage request = CreateRequest(HeaderAppVersionHandler.HeaderName, appVersion);

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateKey_Should_Return_False_When_Headers_Does_Not_Contain_AppVersion()
		{
			// Arrange
			HeaderAppVersionHandler handler = CreateHandler();
			HttpRequestMessage request = CreateRequest("any-name", "any-value");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeFalse();
		}

		private static HeaderAppVersionHandler CreateHandler(Version appVersion = null)
		{
			return new HeaderAppVersionHandler(appVersion ?? new Version());
		}

		private static HttpRequestMessage CreateRequest(string name, object value)
		{
			HttpRequestMessage request = new HttpRequestMessage();
			request.Headers.Add(name, value.ToString());
			return request;
		}
	}
}