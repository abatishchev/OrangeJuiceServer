using System;
using System.Data.Entity.Core;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data.Repository;
using OrangeJuice.Server.Data.Unit;

namespace OrangeJuice.Server.Data.Test.Repository
{
	[TestClass]
	public class EntityRatingRepositoryTest
	{
		#region AddOrUpdate
		[TestMethod]
		public async Task AddOrUpdate_Should_Pass_Rating_Returned_By_RatingUnit_Get_To_RatingUnit_AddOrUpdate()
		{
			// Arrange
			Guid userId = Guid.NewGuid(), productId = Guid.NewGuid();

			Rating rating = new Rating { UserId = userId, ProductId = userId };
			var ratingUnitMock = CreateUnit(rating);

			IRatingRepository repository = CreateRepository(ratingUnitMock.Object);

			// Act
			await repository.AddOrUpdate(userId, productId, 5, "comment");

			// Assert
			ratingUnitMock.Verify(u => u.AddOrUpdate(rating), Times.Once);
		}

		[TestMethod]
		public async Task AddOrUpdate_Should_Pass_Rating_To_RatingUnit_AddOrUpdate_Having_UserId_ProductId_Set_When_RatingUnit_Get_Returns_Null()
		{
			// Arrange
			Guid userId = Guid.NewGuid(), productId = Guid.NewGuid();

			var ratingUnitMock = CreateUnit(null);

			IRatingRepository repository = CreateRepository(ratingUnitMock.Object);

			// Act
			await repository.AddOrUpdate(userId, productId, 5, "comment");

			// Assert
			ratingUnitMock.Verify(u => u.AddOrUpdate(It.Is<Rating>(r => r.UserId == userId && r.ProductId == productId)), Times.Once);
		}
		#endregion

		#region Delete
		[TestMethod]
		public void Delete_Should_Throw_Exception_When_UserUnit_Get_Returns_Null()
		{
			// Arrange
			Guid userId = Guid.NewGuid(), productId = Guid.NewGuid();

			var ratingUnitMock = CreateUnit(null);

			IRatingRepository repository = CreateRepository(ratingUnitMock.Object);

			// Act
			Func<Task> func = () => repository.Delete(userId, productId);

			// Assert
			func.ShouldThrow<ObjectNotFoundException>();
		}
		#endregion

		#region Search
		[TestMethod]
		public async Task Search_Should_Pass_UserId_ProductId_To_RatingUnit_Get()
		{
			// Arrange
			Guid userId = Guid.NewGuid(), productId = Guid.NewGuid();

			var ratingUnitMock = CreateUnit(new Rating());

			IRatingRepository repository = CreateRepository(ratingUnitMock.Object);

			// Act
			await repository.Search(userId, productId);

			// Assert
			ratingUnitMock.Verify(u => u.Get(userId, productId), Times.Once);
		}
		#endregion

		#region SearchAll
		[TestMethod]
		public async Task SearchAll_Should_Pass_ProductId_To_RatingUnit_Get()
		{
			// Arrange
			Guid productId = Guid.NewGuid();

			var ratingUnitMock = new Mock<IRatingUnit>();
			ratingUnitMock.Setup(u => u.Get(productId)).ReturnsAsync(new[] { new Rating(), new Rating() });

			IRatingRepository repository = CreateRepository(ratingUnitMock.Object);

			// Act
			await repository.SearchAll(productId);

			// Assert
			ratingUnitMock.Verify(u => u.Get(productId), Times.Once);
		}
		#endregion

		#region Dispose
		[TestMethod]
		public void Dispose_Should_Call_RatingUnit_Dispose()
		{
			// Arrange
			var ratingUnitMock = new Mock<IRatingUnit>();

			IRatingRepository repository = CreateRepository(ratingUnitMock.Object);

			// Act
			repository.Dispose();

			// Assert
			ratingUnitMock.Verify(u => u.Dispose(), Times.Once);
		}
		#endregion

		#region Helper methods
		private static IRatingRepository CreateRepository(IRatingUnit ratingUnit = null)
		{
			return new EntityRatingRepository(
				ratingUnit ?? new Mock<IRatingUnit>().Object);
		}

		private static Mock<IRatingUnit> CreateUnit(Rating rating)
		{
			var ratingUnitMock = new Mock<IRatingUnit>();
			ratingUnitMock.Setup(u => u.Get(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(rating);
			return ratingUnitMock;
		}
		#endregion
	}
}