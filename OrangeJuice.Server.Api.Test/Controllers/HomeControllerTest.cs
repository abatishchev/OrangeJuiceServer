using System;
using System.Net;
using System.Web.Mvc;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_When_Factory_Is_Null()
		{
			// Arange
			const IApiInfoFactory apiInfoFactory = null;

			// Act
			Action action = () => new HomeController(apiInfoFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("apiInfoFactory");
		}

		[TestMethod]
		public void Index_Should_Return_Status_403_Forbidden()
		{
			// Assign
			HomeController controller = new HomeController(new Mock<IApiInfoFactory>().Object);

			// Act
			HttpStatusCodeResult result = controller.Index();

			// Assert
			result.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
		}

		[TestMethod]
		public void Version_Should_Call_ApiInfoFactory_Create()
		{
			// Arrange
			var factoryMock = new Mock<IApiInfoFactory>();
			HomeController controller = new HomeController(factoryMock.Object);

			// Act
			controller.Version();

			// Assert
			factoryMock.Verify(f => f.Create(), Times.Once);
		}
	}
}