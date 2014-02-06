using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class RatingControllerTest
	{
		#region GetRating
		[TestMethod]
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

		[TestMethod]
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

		[TestMethod]
		public async Task GetRating_Should_Return_Rating_Returned_By_RatingRepository_Search()
		{
			// Arrange
			Guid userId = Guid.NewGuid(), productId = Guid.NewGuid();
			IRating expected = Mock.Of<IRating>();

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(userId, productId)).ReturnsAsync(expected);

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRating(new RatingSearchCriteria { UserId = userId, ProductId = productId });
			IRating actual = ((OkNegotiatedContentResult<IRating>)result).Content;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetRating_Should_Return_Status_Ok()
		{
			// Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.Search(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Mock.Of<IRating>());

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRating(new RatingSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<IRating>>();
		}
		#endregion

		#region GetRatings
		[TestMethod]
		public async Task GetRatings_Should_Return_Status_NotFound_When_RatingRepository_SearchAll_Returns_Null()
		{
			//Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.SearchAll(It.IsAny<Guid>())).ReturnsAsync(null);

			RatingController controller = CreateController();

			// Act
			IHttpActionResult result = await controller.GetRatings(new RatingsSearchCriteria());

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		[TestMethod]
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

		[TestMethod]
		public async Task GetRatings_Should_Return_Ratings_Returned_By_RatingRepository_SearchAll()
		{
			// Arrange
			Guid productId = Guid.NewGuid();
			IRating[] expected = { Mock.Of<IRating>() };

			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.SearchAll(productId)).ReturnsAsync(expected);

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRatings(new RatingsSearchCriteria { ProductId = productId });
			var actual = ((OkNegotiatedContentResult<ICollection<IRating>>)result).Content;

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}

		[TestMethod]
		public async Task GetRatings_Should_Return_Status_Ok()
		{
			// Arrange
			var repositoryMock = new Mock<IRatingRepository>();
			repositoryMock.Setup(r => r.SearchAll(It.IsAny<Guid>())).ReturnsAsync(new[] { Mock.Of<IRating>() });

			RatingController controller = CreateController(repositoryMock.Object);

			// Act
			IHttpActionResult result = await controller.GetRatings(new RatingsSearchCriteria());

			// Assert
			result.Should().BeOfType<OkNegotiatedContentResult<ICollection<IRating>>>();
		}
		#endregion

		#region PostRating
		[TestMethod]
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

		[TestMethod]
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
		[TestMethod]
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

		[TestMethod]
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
			return ControllerFactory.Create<RatingController>(ratingRepository ?? new Mock<IRatingRepository>().Object);
		}
		#endregion
	}
}