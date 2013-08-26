using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Data
{
	[TestClass]
	public class AwsFoodRepositoryTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AwsClientFactory_Is_Null()
		{
			// Arrange
			const IAwsClient awsClient = null;
			const IFoodDescriptionFactory groceryDescriptionFactory = null;

			// Act
			Action action = () => new AwsFoodRepository(awsClient, groceryDescriptionFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("awsClient");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_GroceryDescriptionFactorys_Is_Null()
		{
			// Arrange
			IAwsClient awsClient = new Mock<IAwsClient>().Object;
			const IFoodDescriptionFactory groceryDescriptionFactory = null;

			// Act
			Action action = () => new AwsFoodRepository(awsClient, groceryDescriptionFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("foodDescriptionFactory");
		}
		#endregion

		#region SearchByTitle
		[TestMethod]
		public void SearchByTitle_Should_Throw_Exception_When_Title_Is_Null()
		{
			// Arrange
			const string title = null;
			var repository = CreateRepository();

			// Act
			Func<Task> action = () => repository.SearchByTitle(title);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("title");
		}

		[TestMethod]
		public void SearchByTitle_Should_Throw_Exception_When_Title_Is_Empty()
		{
			// Arrange
			const string title = "";
			var repository = CreateRepository();

			// Act
			Func<Task> action = () => repository.SearchByTitle(title);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("title");
		}
		#endregion

		#region Helper methods
		private static IFoodRepository CreateRepository(IAwsClient awsClient = null, IFoodDescriptionFactory foodDescriptionFactory = null)
		{
			return new AwsFoodRepository(
				awsClient ?? new Mock<IAwsClient>(MockBehavior.Strict).Object,
				foodDescriptionFactory ?? new Mock<IFoodDescriptionFactory>(MockBehavior.Strict).Object);
		}
		#endregion
	}
}