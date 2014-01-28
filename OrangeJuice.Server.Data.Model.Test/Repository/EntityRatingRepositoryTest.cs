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
		#region Test methods
		[TestMethod]
		public async Task AddOrUpdate_Should_Call_UserUnit_Get_When_RatingUnit_Get_Returns_Null()
		{
			// Arrange
			RatingId ratingId = new RatingId();

			var ratingUnitMock = new Mock<IRatingUnit>();
			ratingUnitMock.Setup(u => u.Get(ratingId)).ReturnsAsync(null);

			var userUnitMock = new Mock<IUserUnit>();

			IRatingRepository repository = CreateRepository(ratingUnitMock.Object, userUnitMock.Object);

			// Act
			await repository.AddOrUpdate(ratingId, 5, "comment");

			// Assert
			userUnitMock.Verify(u => u.Get(ratingId.UserId), Times.Once);
		}

		public async Task AddOrUpdate_Should_Not_Call_UserUnit_Get_When_RatingUnit_Get_Returns_Not_Null()
		{
			// Arrange
			RatingId ratingId = new RatingId();

			var ratingUnitMock = new Mock<IRatingUnit>();
			ratingUnitMock.Setup(u => u.Get(ratingId)).ReturnsAsync(new Rating());

			var userUnitMock = new Mock<IUserUnit>();

			IRatingRepository repository = CreateRepository(ratingUnitMock.Object, userUnitMock.Object);

			// Act
			await repository.AddOrUpdate(ratingId, 5, "comment");

			// Assert
			userUnitMock.Verify(u => u.Get(ratingId.UserId), Times.Never);
		}

		[TestMethod]
		public async Task AddOrUpdate_Should_Pass_Result_Of_RatingUnit_Get_To_RatingUnit_AddOrUpdate()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void Delete_Should_Throw_Exception_When_UserUnit_Get_Returns_Null()
		{
			// Arrange
			RatingId ratingId = new RatingId();

			var ratingUnitMock = new Mock<IRatingUnit>();
			ratingUnitMock.Setup(u => u.Get(ratingId)).ReturnsAsync(null);

			IRatingRepository repository = CreateRepository(ratingUnitMock.Object);

			// Act
			Func<Task> func = () => repository.Delete(ratingId);

			// Assert
			func.ShouldThrow<ObjectNotFoundException>();
		}

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

		[TestMethod]
		public void Dispose_Should_Call_UserUnit_Dispose()
		{
			// Arrange
			var userUnitMock = new Mock<IUserUnit>();

			IRatingRepository repository = CreateRepository(userUnit: userUnitMock.Object);

			// Act
			repository.Dispose();

			// Assert
			userUnitMock.Verify(u => u.Dispose(), Times.Once);
		}
		#endregion

		#region Helper methods
		private static IRatingRepository CreateRepository(IRatingUnit ratingUnit = null, IUserUnit userUnit = null)
		{
			return new EntityRatingRepository(
				ratingUnit ?? new Mock<IRatingUnit>().Object,
				userUnit ?? new Mock<IUserUnit>().Object);
		}
		#endregion
	}
}