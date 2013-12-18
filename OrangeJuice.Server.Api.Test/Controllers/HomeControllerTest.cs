using System.Net;
using System.Net.Http;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Controllers;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		[TestMethod]
		public void Get_Should_Return_Status_403_Forbidden()
		{
			// Assign
			const HttpStatusCode expected = HttpStatusCode.Forbidden;

			HomeController controller = new HomeController();

			// Act
			HttpResponseMessage message = controller.Get();
			HttpStatusCode actual = message.StatusCode;

			// Assert
			actual.Should().Be(expected);
		}
	}
}