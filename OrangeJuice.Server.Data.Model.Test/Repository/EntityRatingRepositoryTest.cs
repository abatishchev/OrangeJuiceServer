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
		public void AddOrUpdate_Should_Throw_Exception_When_Users_Does_Not_Contain_User_With_UserGuid()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			const string productId = "productid";
			const byte value = 5;

			IRatingRepository repository = CreateRepository();

			// Act
			Func<Task> func = () => repository.AddOrUpdate(userGuid, productId, value);

			// Assert
			func.ShouldThrow<ObjectNotFoundException>();
		}
		#endregion

		#region Helper methods
		private static EntityRatingRepository CreateRepository(IRatingUnit ratingUnit = null, IUserUnit userUnit = null)
		{
			return new EntityRatingRepository(
				ratingUnit ?? new Mock<IRatingUnit>().Object,
				userUnit ?? new Mock<IUserUnit>().Object);
		}
		#endregion
	}
}