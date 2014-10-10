using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using FluentAssertions;

using OrangeJuice.Server.Api.Filters;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Filters
{
	public class ValidModelActionFilterTest
	{
		#region Test methods
		[Fact]
		public void OnActionExecuting_Should_Assign_ActionContext_Response_StatusCode_To_BadRequest_When_Model_Is_Not_Valid()
		{
			// Arrange
			HttpActionContext actionContext = CreateActionContext();
			actionContext.ModelState.AddModelError("", "");

			ActionFilterAttribute filter = new ValidModelActionFilter();

			// Act
			filter.OnActionExecuting(actionContext);

			// Assert
			actionContext.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Fact]
		public void OnActionExecuting_Should_Not_Change_ActionContext_Response_When_Model_Is_Valid()
		{
			// Arrange
			HttpActionContext actionContext = CreateActionContext();
			HttpResponseMessage expected = actionContext.Response;

			ActionFilterAttribute filter = new ValidModelActionFilter();

			// Act
			filter.OnActionExecuting(actionContext);
			HttpResponseMessage actual = actionContext.Response;

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods
		private static HttpActionContext CreateActionContext()
		{
			HttpControllerContext controllerContext = new HttpControllerContext
			{
				Request = new HttpRequestMessage()
			};

			var actionContext = new HttpActionContext(controllerContext, new ReflectedHttpActionDescriptor());
			return actionContext;
		}
		#endregion
	}
}