using System.Web.Http.Results;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class VersionControllerTest
	{
		[TestMethod]
		public void GetVersion_Should_Return_ApiVersion()
		{
			// Arrange
			ApiVersion expected = new ApiVersion();

			VersionController controller = ControllerFactory.Create<VersionController>(expected);

			// Act
			var result = (OkNegotiatedContentResult<ApiVersion>)controller.GetVersion();
			ApiVersion actual = result.Content;

			// Assert
			actual.Should().Be(expected);
		}
	}
}