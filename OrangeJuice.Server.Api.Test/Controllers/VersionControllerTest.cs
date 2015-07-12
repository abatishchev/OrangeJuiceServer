using System;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Controllers;
using OrangeJuice.Server.Data.Models;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	public class VersionControllerTest
	{
		#region Test methods
		[Theory]
		[InlineData(typeof(VersionController))]
		[InlineData(typeof(FSharp.Controllers.VersionController))]
		public void GetVersion_Should_Return_ApiVersion(Type type)
		{
			// Arrange
			ApiVersion expected = new ApiVersion();

			IVersionController controller = CreateController(type, expected);

			// Act
			IHttpActionResult result = controller.GetVersion();
			ApiVersion actual = ((OkNegotiatedContentResult<ApiVersion>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static IVersionController CreateController(Type type, ApiVersion apiVersion)
		{
			return (IVersionController)ControllerFactory.Create(type, apiVersion);
		}
		#endregion
	}
}