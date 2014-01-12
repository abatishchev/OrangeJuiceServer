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
		public void AddOrUpdate_Should_Throw_Exception_When_UserUnit_Get_Returns_Null()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			const string productId = "productid";
			const byte value = 5;

			var ratingUnitMock = new Mock<IRatingUnit>();
			ratingUnitMock.Setup(u => u.Get(userGuid, productId)).ReturnsAsync(null);

			IRatingRepository repository = CreateRepository();

			// Act
			Func<Task> func = () => repository.AddOrUpdate(userGuid, productId, value);

			// Assert
			func.ShouldThrow<ObjectNotFoundException>();
		}

		[TestMethod]
		public void Delete_Should_Throw_Exception_When_UserUnit_Get_Returns_Null()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			const string productId = "productid";

			var ratingUnitMock = new Mock<IRatingUnit>();
			ratingUnitMock.Setup(u => u.Get(userGuid, productId)).ReturnsAsync(null);

			IRatingRepository repository = CreateRepository(ratingUnitMock.Object);

			// Act
			Func<Task> func = () => repository.Delete(userGuid, productId);

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