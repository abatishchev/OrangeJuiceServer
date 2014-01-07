using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class RatingControllerTest
	{
		#region GetRating
		[TestMethod]
		public async Task GetRating_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			RatingController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			// Act
			IHttpActionResult result = await controller.GetRating(new RatingSearchCriteria());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task GetRating_Should_Return_NotFound_When_RatingRepository_Returns_Null()
		{
			//Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(null);

			RatingController controller = CreateController();

			// Act
			IHttpActionResult result = await controller.GetRating(new RatingSearchCriteria());

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		[TestMethod]
		public async Task GetRating_Should_Pass_UserGuid_And_ProductId_To_RatingRepository_Search()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			const string productid = "NewProductId";

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(userGuid, productid)).ReturnsAsync(null);

			RatingController controller = CreateController(repositoryMock.Object);
			RatingSearchCriteria searchCriteria = new RatingSearchCriteria { UserGuid = userGuid, Productid = productid };

			// Act
			await controller.GetRating(searchCriteria);

			// Assert
			repositoryMock.Verify(r => r.Search(userGuid, productid), Times.Once());
		}

		[TestMethod]
		public async Task GetRating_Should_Return_Rating_By_Specified_UserGuid_And_ProductId()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			const string productid = "NewProductId";
			IRating expected = CreateRating(userGuid, productid);

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(userGuid, productid)).ReturnsAsync(expected);

			RatingController controller = CreateController(repositoryMock.Object);
			RatingSearchCriteria searchCriteria = new RatingSearchCriteria { UserGuid = userGuid, Productid = productid };

			// Act
			var result = (OkNegotiatedContentResult<IRating>)await controller.GetRating(searchCriteria);
			IRating actual = result.Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetRating_Should_Return_Ok()
		{
			// Arrange
			IRating rating = CreateRating();
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(rating);

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRating(new RatingSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<IRating>>();
		}
		#endregion

		#region PostRating
		[TestMethod]
		public async Task PostRating_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			RatingController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			// Act
			IHttpActionResult result = await controller.PostRating(new RatingInformation());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task PostRating_Should_Pass_UserGuid_And_ProductId_And_Value_To_RatingRepository_Delete()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			const string productid = "NewProductId";
			const int value = 5;

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(userGuid, productid)).Returns(Task.Delay(0));

			RatingController controller = CreateController(repositoryMock.Object);
			RatingInformation ratingInformation = new RatingInformation { Productid = productid, UserGuid = userGuid, Value = value };

			// Act
			await controller.PostRating(ratingInformation);

			// Assert
			repositoryMock.Verify(r => r.AddOrUpdate(userGuid, productid, value), Times.Once());
		}

		[TestMethod]
		public async Task PostRating_Should_Return_Ok()
		{
			// Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.Delay(0));

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PostRating(new RatingInformation());

			// Assert
			result.Should().BeOfType<OkResult>();
		}
		#endregion

		#region DeleteRating
		[TestMethod]
		public async Task DeleteRating_Should_Return_InvalidModelState_When_Model_Not_IsValid()
		{
			// Arrange
			RatingController controller = CreateController();
			controller.ModelState.AddModelError("", "");

			// Act
			IHttpActionResult result = await controller.DeleteRating(new RatingSearchCriteria());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task DeleteRating_Should_Pass_UserGuid_And_ProductId_To_RatingRepository_Delete()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			const string productid = "NewProductId";

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(userGuid, productid)).Returns(Task.Delay(0));

			RatingController controller = CreateController(repositoryMock.Object);
			RatingSearchCriteria searchCriteria = new RatingSearchCriteria { UserGuid = userGuid, Productid = productid };

			// Act
			await controller.DeleteRating(searchCriteria);

			// Assert
			repositoryMock.Verify(r => r.Delete(userGuid, productid), Times.Once());
		}

		[TestMethod]
		public async Task DeleteRating_Should_Return_Ok()
		{
			// Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.Delay(0));

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.DeleteRating(new RatingSearchCriteria());

			// Assert
			result.Should().BeOfType<OkResult>();
		}
		#endregion

		#region Helper methods
		private static RatingController CreateController(IRatingRepository ratingRepository = null)
		{
			return ControllerFactory.Create<RatingController>(ratingRepository ?? new Mock<IRatingRepository>().Object);
		}

		private static IRating CreateRating(Guid? userGuid = null, string productId = null)
		{
			var ratingMock = new Mock<IRating>();
			ratingMock.Setup(u => u.UserGuid).Returns(userGuid ?? Guid.NewGuid());
			ratingMock.Setup(u => u.ProductId).Returns(productId ?? "NewProductId");
			return ratingMock.Object;
		}
		#endregion
	}
}