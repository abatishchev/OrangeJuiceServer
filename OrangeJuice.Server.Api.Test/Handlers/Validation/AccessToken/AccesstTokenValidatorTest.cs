using System.Net.Http;

using FluentAssertions;

using OrangeJuice.Server.Api.Handlers.Validation;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Handlers.Validation
{
	public class AccesstTokenValidatorTest
	{
		#region Test methods
		[Fact]
		public void IsValid_Should_Return_True_When_Headers_Contains_Authorization_And_It_Is_Bearer()
		{
			// Arrange
			HeaderAccesstTokenValidator handler = new HeaderAccesstTokenValidator();
			HttpRequestMessage request = CreateRequest("Authorization", "Bearer test");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeTrue();
		}

		[Fact]
		public void IsValid_Should_Return_False_When_Headers_Does_Not_Contain_Authorization()
		{
			// Arrange
			HeaderAccesstTokenValidator handler = new HeaderAccesstTokenValidator();
			HttpRequestMessage request = CreateRequest("any-name", "any-value");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeFalse();
		}
		#endregion

		#region Helper methods
		private static HttpRequestMessage CreateRequest(string name, object value)
		{
			HttpRequestMessage request = new HttpRequestMessage();
			request.Headers.Add(name, value.ToString());
			return request;
		}
		#endregion
	}
}