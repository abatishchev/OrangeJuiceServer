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
	public class UnhandledExceptionFilterTest
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
		public void OnException_Should_Include_Include_Error_Detail_When_Assigns_Exception_To_Response()
		{
			// Arrange
			Exception exception = new InvalidOperationException();
			var filter = new UnhandledExceptionFilterAttribute(typeof(InvalidOperationException));
			HttpActionExecutedContext context = CreateContext(exception);

			// Act
			filter.OnException(context);
			HttpError httpError = context.Response.Content.GetValue<HttpError>();

			// Assert
			httpError.ExceptionType.Should().NotBeNull();
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
			action.ShouldThrow<ArgumentException>()
				  .And.ParamName.Should().Be("context");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_Type_Is_Null()
		{
			// Arrange
			const Type exceptionType = null;

			// Act
			Action action = () => new UnhandledExceptionFilterAttribute(exceptionType);

			// Assert
			action.ShouldThrow<ArgumentException>()
				  .And.ParamName.Should().Be("exceptionType");
		}
		#endregion

		#region Helper methods
		private static HttpActionExecutedContext CreateContext(Exception ex = null)
		{
			return new HttpActionExecutedContext(
					new HttpActionContext(
							new HttpControllerContext(
									new HttpConfiguration(),
									new Mock<IHttpRouteData>().Object,
									new HttpRequestMessage()),
							new Mock<HttpActionDescriptor>().Object),
					ex)
			{
				Response = new HttpResponseMessage()
			};
		}
		#endregion
	}
}