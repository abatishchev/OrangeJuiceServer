using System;
using System.Net.Http;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Handlers.Validation;

namespace OrangeJuice.Server.Api.Test.Handlers.Validation
{
	[TestClass]
	public class HeaderAppVersionValidatorTest
	{
		#region Test methods
		[TestMethod]
		public void IsValid_Should_Return_True_When_Headers_Contains_AppVersion()
		{
			// Arrange
			Version appVersion = new Version();
			HeaderAppVersionValidator handler = CreateHandler(appVersion);
			HttpRequestMessage request = CreateRequest(AppVersion.ElementName, appVersion);

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeTrue();
		}

		[TestMethod]
		public void IsValid_Should_Return_False_When_Headers_Does_Not_Contain_AppVersion()
		{
			// Arrange
			HeaderAppVersionValidator handler = CreateHandler();
			HttpRequestMessage request = CreateRequest("any-name", "any-value");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeFalse();
		}
		#endregion

		#region Helper methods
		private static HeaderAppVersionValidator CreateHandler(Version appVersion = null)
		{
			return new HeaderAppVersionValidator(appVersion ?? new Version());
		}

		private static HttpRequestMessage CreateRequest(string name, object value)
		{
			HttpRequestMessage request = new HttpRequestMessage();
			request.Headers.Add(name, value.ToString());
			return request;
		}
		#endregion
	}
}