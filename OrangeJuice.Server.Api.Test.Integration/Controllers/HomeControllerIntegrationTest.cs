using System.Net;
using System.Threading.Tasks;

using FluentAssertions;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration.Controllers
{
	public class HomeControllerIntegrationTest
	{
		[Fact]
		public async Task GetRoot_Should_Return_Status_NotFound()
		{
			// Arrange
			var client = HttpClientFactory.Create();

			// Act
			var response = await client.GetAsync("/api");

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task GetVersion_Should_Return_Status_Ok()
		{
			// Arrange
			var client = HttpClientFactory.Create();

			// Act
			var response = await client.GetAsync("/api/version");

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}
	}
}