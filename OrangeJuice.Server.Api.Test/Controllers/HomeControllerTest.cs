using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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
		public async Task GetVersion_Should_Return_Status_403_Forbidden()
		{
			// Assign
			const HttpStatusCode expected = HttpStatusCode.Forbidden;

			HomeController controller = CreateController();

			// Act
			HttpResponseMessage message = await controller.GetVersion();
			HttpStatusCode actual = message.Content.GetValue<HttpStatusCode>();

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetVersion_Should_Call_ApiInfoFactory_Create()
		{
			// Arrange
			var factoryMock = new Mock<IApiInfoFactory>();
			HomeController controller = CreateController(factoryMock.Object);

			// Act
			await controller.GetVersion();

			// Assert
			factoryMock.Verify(f => f.Create(), Times.Once());
		}

		private static HomeController CreateController(IApiInfoFactory factory = null)
		{
			return ControllerFactory.Create<HomeController>(factory ?? new Mock<IApiInfoFactory>().Object);
		}
	}
}