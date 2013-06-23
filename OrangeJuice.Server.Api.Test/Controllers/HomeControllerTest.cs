using System.Net;
using System.Web.Mvc;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Controllers;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		[TestMethod]
		public void Index_Should_Return_403_Forbidden()
		{
			// Assign
			HomeController controller = new HomeController();

			// Act
			HttpStatusCodeResult result = controller.Index();

			// Assert
			result.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
		}
	}
}