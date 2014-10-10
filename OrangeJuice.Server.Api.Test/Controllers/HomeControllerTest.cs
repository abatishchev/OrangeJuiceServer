using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Data;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	public class HomeControllerTest
	{
		#region Test methods
		[Fact]
		public void Get_Should_Return_Status_NotFound()
		{
			// Assign
			HomeController controller = CreateController();

			// Act
			IHttpActionResult result = controller.Get();

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		[Fact]
		public void GetVersion_Should_Return_ApiVersion()
		{
			// Arrange
			ApiVersion expected = new ApiVersion();

			HomeController controller = CreateController(expected);

			// Act
			IHttpActionResult result = controller.GetVersion();
			ApiVersion actual = ((OkNegotiatedContentResult<ApiVersion>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static HomeController CreateController(ApiVersion apiVersion = null)
		{
			return ControllerFactory<HomeController>.Create(apiVersion ?? new ApiVersion());
		}
		#endregion
	}
}