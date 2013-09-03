using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Test;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class VersionControllerTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_When_Factory_Is_Null()
		{
			// Arange
			const IApiVersionFactory apiInfoFactory = null;

			// Act
			Action action = () => new VersionController(apiInfoFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("apiVersionFactory");
		}

		[TestMethod]
		public async Task GetVersion_Should_Call_ApiInfoFactory_Create()
		{
			// Arrange
			var factoryMock = new Mock<IApiVersionFactory>();
			factoryMock.Setup(f => f.Create()).ReturnsAsync(new ApiVersion());

			VersionController controller = ControllerFactory.Create<VersionController>(factoryMock.Object);

			// Act
			await controller.GetVersion();

			// Assert
			factoryMock.Verify(f => f.Create(), Times.Once());
		}
	}
}