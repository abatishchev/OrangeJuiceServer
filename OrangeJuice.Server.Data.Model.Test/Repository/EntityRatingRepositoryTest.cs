using System;
using System.Data.Entity.Core;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data.Model.Repository;

namespace OrangeJuice.Server.Data.Model.Test.Repository
{
	[TestClass]
	public class EntityRatingRepositoryTest
	{
		#region Test methods
		[TestMethod]
		public void AddOrUpdate_Should_Throw_ObjectNotFoundException_When_Users_Does_Not_Contain_User_With_UserGuid()
		{
			// Arrange
			Guid userGuid = Guid.NewGuid();
			const string productId = "productid";
			const byte value = 5;

			var users = new[] { new User() };

			var setMock = DbSetFactory.Create(users);

			var containerMock = new Mock<ModelContainer>();
			containerMock.Setup(c => c.Users).Returns(setMock);

			IRatingRepository repository = CreateRepository(containerMock);

			// Act
			Func<Task> func = () => repository.AddOrUpdate(userGuid, productId, value);

			// Assert
			func.ShouldThrow<ObjectNotFoundException>();
		}
		#endregion

		#region Helper methods
		private static EntityRatingRepository CreateRepository(IMock<ModelContainer> containerMock)
		{
			return new EntityRatingRepository(new ProxyFactory<ModelContainer>(() => containerMock.Object));
		}
		#endregion
	}
}