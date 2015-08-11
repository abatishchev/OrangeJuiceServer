using System;
using System.Net.Http;
using System.Net.Http.Headers;

using FluentAssertions;

using OrangeJuice.Server.Api.Handlers;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Handlers
{
	public class AcceptHeaderValidatorTest
	{
		#region Test methods
		[Fact]
		public void IsValid_Should_Return_True_When_Headers_Contain_Accept()
		{
			// Arrange
			Version appVersion = new Version();
			AcceptHeaderValidator handler = CreateHandler(appVersion);
			HttpRequestMessage request = CreateRequest(appVersion);

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeTrue();
		}

		[Fact]
		public void IsValid_Should_Return_False_When_Headers_Do_Not_Contain_Accept()
		{
			// Arrange
			AcceptHeaderValidator handler = CreateHandler();
			HttpRequestMessage request = CreateRequest("any-value");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeFalse();
		}
		#endregion

		#region Helper methods
		private static AcceptHeaderValidator CreateHandler(Version appVersion = null)
		{
			return new AcceptHeaderValidator(appVersion);
		}

		private static HttpRequestMessage CreateRequest(object value)
		{
			HttpRequestMessage request = new HttpRequestMessage();
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(String.Format("application/vnd.orangejuice.v{0}+json", value)));
			return request;
		}
		#endregion
	}
}