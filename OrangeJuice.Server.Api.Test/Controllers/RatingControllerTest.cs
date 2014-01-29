using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

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
			IHttpActionResult result = await controller.GetRating(new RatingId());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task GetRating_Should_Return_NotFound_When_RatingRepository_Returns_Null()
		{
			//Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<RatingId>())).ReturnsAsync(null);

			RatingController controller = CreateController();

			// Act
			IHttpActionResult result = await controller.GetRating(new RatingId());

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		[TestMethod]
		public async Task GetRating_Should_Pass_UserId_And_ProductId_To_RatingRepository_Search()
		{
			// Arrange
			RatingId ratingId = new RatingId();

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(ratingId)).ReturnsAsync(null);

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			await controller.GetRating(ratingId);

			// Assert
			repositoryMock.Verify(r => r.Search(ratingId), Times.Once);
		}

		[TestMethod]
		public async Task GetRating_Should_Return_Rating_By_Specified_UserId_And_ProductId()
		{
			// Arrange
			RatingId ratingId = new RatingId();
			IRating expected = new Mock<IRating>().Object;

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(ratingId)).ReturnsAsync(expected);

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRating(ratingId);
			IRating actual = ((OkNegotiatedContentResult<IRating>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetRating_Should_Return_Ok()
		{
			// Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<RatingId>())).ReturnsAsync(new Mock<IRating>().Object);

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRating(new RatingId());

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
			IHttpActionResult result = await controller.PostRating(new Rating());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task PostRating_Should_Pass_UserId_And_ProductId_And_Value_To_RatingRepository_Delete()
		{
			// Arrange
			RatingId ratingId = new RatingId();
			const int value = 5;
			const string comment = "comment";

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(ratingId)).Returns(Task.Delay(0));

			RatingController controller = CreateController(repositoryMock.Object);
			Rating ratingInformation = new Rating { RatingId = ratingId, Value = value, Comment = comment };

			// Act
			await controller.PostRating(ratingInformation);

			// Assert
			repositoryMock.Verify(r => r.AddOrUpdate(ratingId, value, comment), Times.Once);
		}

		[TestMethod]
		public async Task PostRating_Should_Return_Ok()
		{
			// Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(It.IsAny<RatingId>())).Returns(Task.Delay(0));

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PostRating(new Rating());

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
			IHttpActionResult result = await controller.DeleteRating(new RatingId());

			// Assert
			result.Should().BeOfType<InvalidModelStateResult>();
		}

		[TestMethod]
		public async Task DeleteRating_Should_Pass_UserId_And_ProductId_To_RatingRepository_Delete()
		{
			// Arrange
			RatingId ratingId = new RatingId();

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(ratingId)).Returns(Task.Delay(0));

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			await controller.DeleteRating(ratingId);

			// Assert
			repositoryMock.Verify(r => r.Delete(ratingId), Times.Once);
		}

		[TestMethod]
		public async Task DeleteRating_Should_Return_Ok()
		{
			// Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(It.IsAny<RatingId>())).Returns(Task.Delay(0));

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.DeleteRating(new RatingId());

			// Assert
			result.Should().BeOfType<OkResult>();
		}
		#endregion

		#region Helper methods
		private static RatingController CreateController(IRatingRepository ratingRepository = null)
		{
			return ControllerFactory.Create<RatingController>(ratingRepository ?? new Mock<IRatingRepository>().Object);
		}
		#endregion
	}
}