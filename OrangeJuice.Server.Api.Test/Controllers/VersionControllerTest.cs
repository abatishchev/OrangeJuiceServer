using System;
using System.Net.Http;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class VersionControllerTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_When_Factory_Is_Null()
		{
			// Arange
			const ApiVersion apiVersion = null;

			// Act
			Action action = () => new VersionController(apiVersion);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("apiVersion");
		}

		[TestMethod]
		public void GetVersion_Should_Return_ApiVersion_Returned_By_ApiInfoFactory_Create()
		{
			// Arrange
			ApiVersion expected = new ApiVersion();

			var factoryMock = new Mock<IApiVersionFactory>();
			factoryMock.Setup(f => f.Create()).Returns(expected);

			VersionController controller = ControllerFactory.Create<VersionController>(factoryMock.Object);

			// Act
			HttpResponseMessage message = controller.GetVersion();
			ApiVersion actual = message.Content.GetValue<ApiVersion>();

			// Assert
			actual.Should().Be(expected);
		}
	}
}