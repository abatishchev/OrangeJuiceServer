using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Controllers;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		[TestMethod]
		public void Get_Should_Return_Status_NotFound()
		{
			// Assign
			HomeController controller = new HomeController();

			// Act
			IHttpActionResult result = controller.Get();

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}
	}
}