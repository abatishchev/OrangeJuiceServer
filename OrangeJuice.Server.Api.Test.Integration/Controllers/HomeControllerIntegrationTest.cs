using System.Net;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Api.Test.Integration.Controllers
{
	[TestClass]
	public class HomeControllerIntegrationTest
	{
		[TestMethod]
		public async Task Get_Root_Should_Return_NotFound()
		{
			// Arrange
			var client = HttpClientFactory.Create();

			// Act
			var response = await client.GetAsync("/api");

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[TestMethod]
		public async Task Get_Root_Home_Should_Return_NotFound()
		{
			// Arrange
			var client = HttpClientFactory.Create();

			// Act
			var response = await client.GetAsync("/api/home");

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[TestMethod]
		public async Task Get_Root_Home_Version_Should_Return_NotFound()
		{
			// Arrange
			var client = HttpClientFactory.Create();

			// Act
			var response = await client.GetAsync("/api/home/version");

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}
	}
}