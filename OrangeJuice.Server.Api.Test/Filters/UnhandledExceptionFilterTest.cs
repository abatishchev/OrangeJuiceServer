using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Routing;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Filters;

namespace OrangeJuice.Server.Api.Test.Filters
{
	[TestClass]
	public class UnhandledExceptionFilterAttributeTest
	{
		#region Test methods
		[TestMethod]
		public void OnException_Should_Assign_Exception_To_Response_When_Exception_Equals_Target()
		{
			// Arrange
			Exception exception = new InvalidOperationException();
			var filter = new UnhandledExceptionFilterAttribute(typeof(InvalidOperationException));
			HttpActionExecutedContext context = CreateContext(exception);

			// Act
			filter.OnException(context);
			HttpError httpError = context.Response.Content.GetValue<HttpError>();

			// Assert
			httpError.Should().NotBeNull();
		}

		[TestMethod]
		public void OnException_Should_Not_Assign_Exception_To_Response_When_Exception_Not_Equals_Target()
		{
			// Arrange
			Exception exception = new ArgumentException();
			var filter = new UnhandledExceptionFilterAttribute(typeof(InvalidOperationException));
			HttpActionExecutedContext context = CreateContext(exception);

			// Act
			filter.OnException(context);
			HttpError httpError = context.Response.Content.GetValue<HttpError>();

			// Assert
			httpError.Should().BeNull();
		}

		[TestMethod]
		public void OnException_Should_Throw_Exception_When_Context_Is_Null()
		{
			// Arrange
			var filter = new UnhandledExceptionFilterAttribute(typeof(Exception));
			const HttpActionExecutedContext context = null;

			// Act
			Action action = () => filter.OnException(context);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("context");
		}
		#endregion

		#region Helper methods
		private static HttpActionExecutedContext CreateContext(Exception ex = null)
		{
			return new HttpActionExecutedContext(
				new HttpActionContext(
					new HttpControllerContext(
						new HttpConfiguration(),
						new Mock<IHttpRouteData>(MockBehavior.Strict).Object,
						new HttpRequestMessage()),
					new Mock<HttpActionDescriptor>(MockBehavior.Strict).Object),
				null)
			{
				Response = new HttpResponseMessage(),
				Exception = ex
			};
		}
		#endregion
	}
}