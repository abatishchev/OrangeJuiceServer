using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;
using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	public class RatingControllerTest
	{
		#region GetRating
		[Fact]
		public void GetRating_Should_Should_Throw_Exception_When_SearchCriteria_Is_Null()
		{
			// Arrange
			RatingController controller = CreateController();

			// Act
			Func<Task> task = () => controller.GetRating(null);

			// Assert
			task.ShouldThrow<ArgumentNullException>();
		}

		[Fact]
		public async Task GetRating_Should_Return_Status_NotFound_When_RatingRepository_Search_Returns_Null()
		{
			//Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(null);

			RatingController controller = CreateController();

			// Act
			IHttpActionResult result = await controller.GetRating(new RatingSearchCriteria());

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		[Fact]
		public async Task GetRating_Should_Pass_UserId_And_ProductId_To_RatingRepository_Search()
		{
			// Arrange
			Guid userId = Guid.NewGuid(), productId = Guid.NewGuid();

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(userId, productId)).ReturnsAsync(null);

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			await controller.GetRating(new RatingSearchCriteria { UserId = userId, ProductId = productId });

			// Assert
			repositoryMock.Verify(r => r.Search(userId, productId), Times.Once);
		}

		[Fact]
		public async Task GetRating_Should_Return_Rating_Returned_By_RatingRepository_Search()
		{
			// Arrange
			Guid userId = Guid.NewGuid(), productId = Guid.NewGuid();
			Rating expected = new Rating();

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(userId, productId)).ReturnsAsync(expected);

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRating(new RatingSearchCriteria { UserId = userId, ProductId = productId });
			Rating actual = ((OkNegotiatedContentResult<Rating>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[Fact]
		public async Task GetRating_Should_Return_Status_Ok()
		{
			// Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new Rating());

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRating(new RatingSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<Rating>>();
		}
		#endregion

		#region GetRatings
		[Fact]
		public void GetRatings_Should_Should_Throw_Exception_When_SearchCriteria_Is_Null()
		{
			// Arrange
			RatingController controller = CreateController();

			// Act
			Func<Task> task = () => controller.GetRatings(null);

			// Assert
			task.ShouldThrow<ArgumentNullException>();
		}

		[Fact]
		public async Task GetRatings_Should_Return_Status_NotFound_When_RatingRepository_SearchAll_Returns_Null()
		{
			//Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.SearchAll(It.IsAny<Guid>())).ReturnsAsync(null);

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRatings(new RatingsSearchCriteria());

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		[Fact]
		public async Task GetRatings_Should_Pass_ProductId_To_RatingRepository_SearchAll()
		{
			// Arrange
			Guid productId = Guid.NewGuid();

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.SearchAll(productId)).ReturnsAsync(null);

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			await controller.GetRatings(new RatingsSearchCriteria { ProductId = productId });

			// Assert
			repositoryMock.Verify(r => r.SearchAll(productId), Times.Once);
		}

		[Fact]
		public async Task GetRatings_Should_Return_Ratings_Returned_By_RatingRepository_SearchAll()
		{
			// Arrange
			Guid productId = Guid.NewGuid();
			Rating[] expected = {new Rating() };

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.SearchAll(productId)).ReturnsAsync(expected);

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRatings(new RatingsSearchCriteria { ProductId = productId });
			var actual = ((OkNegotiatedContentResult<Rating[]>)result).Content;

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public async Task GetRatings_Should_Return_Status_Ok()
		{
			// Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.SearchAll(It.IsAny<Guid>())).ReturnsAsync(new[] { new Rating() });

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRatings(new RatingsSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<Rating[]>>();
		}
		#endregion

		#region PostRating
		[Fact]
		public void PostRating_Should_Should_Throw_Exception_When_RatingModel_Is_Null()
		{
			// Arrange
			RatingController controller = CreateController();

			// Act
			Func<Task> task = () => controller.PostRating(null);

			// Assert
			task.ShouldThrow<ArgumentNullException>();
		}

		[Fact]
		public async Task PostRating_Should_Pass_UserId_And_ProductId_And_Value_To_RatingRepository_Delete()
		{
			// Arrange
			Guid userId = Guid.NewGuid(), productId = Guid.NewGuid();
			const int value = 5;
			const string comment = "comment";

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(userId, productId)).Returns(Task.Delay(0));

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			await controller.PostRating(new RatingModel { UserId = userId, ProductId = productId, Value = value, Comment = comment });

			// Assert
			repositoryMock.Verify(r => r.AddOrUpdate(userId, productId, value, comment), Times.Once);
		}

		[Fact]
		public async Task PostRating_Should_Return_Status_Ok()
		{
			// Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.Delay(0));

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.PostRating(new RatingModel());

			// Assert
			result.Should().BeOfType<OkResult>();
		}
		#endregion

		#region DeleteRating
		[Fact]
		public void DeleteRating_Should_Should_Throw_Exception_When_SearchCriteria_Is_Null()
		{
			// Arrange
			RatingController controller = CreateController();

			// Act
			Func<Task> task = () => controller.DeleteRating(null);

			// Assert
			task.ShouldThrow<ArgumentNullException>();
		}

		[Fact]
		public async Task DeleteRating_Should_Pass_UserId_And_ProductId_To_RatingRepository_Delete()
		{
			// Arrange
			Guid userId = Guid.NewGuid(), productId = Guid.NewGuid();

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(userId, productId)).Returns(Task.Delay(0));

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			await controller.DeleteRating(new RatingSearchCriteria { UserId = userId, ProductId = productId });

			// Assert
			repositoryMock.Verify(r => r.Delete(userId, productId), Times.Once);
		}

		[Fact]
		public async Task DeleteRating_Should_Return_Status_Ok()
		{
			// Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.Delay(0));

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
			return ControllerFactory<RatingController>.Create(ratingRepository ?? new Mock<IRatingRepository>().Object);
		}
		#endregion
	}
}