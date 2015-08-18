using System.Net;
using System.Threading.Tasks;

using FluentAssertions;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration.Controllers
{
	public class AuthorizationIntegrationTest
	{
		[Fact]
		public async Task GetToken_Should_Return_Status_Ok()
		{
			// Arrange
			var client = HttpClientFactory.Create();

			// Act
			var response = await client.GetAsync("/api/auth/token");

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}
	}
}