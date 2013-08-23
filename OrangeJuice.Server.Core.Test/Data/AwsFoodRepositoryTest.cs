using System;

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
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AwsClientFactory_Is_Null()
		{
			// Arrange
			const IAwsClientFactory awsClientFactory = null;
			const IFoodDescriptionFactory groceryDescriptionFactory = null;

			// Act
			Action action = () => new AwsFoodRepository(awsClientFactory, groceryDescriptionFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("awsClientFactory");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_GroceryDescriptionFactorys_Is_Null()
		{
			// Arrange
			IAwsClientFactory awsClientFactory = new Mock<IAwsClientFactory>().Object;
			const IFoodDescriptionFactory groceryDescriptionFactory = null;

			// Act
			Action action = () => new AwsFoodRepository(awsClientFactory, groceryDescriptionFactory);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("foodDescriptionFactory");
		}
	}
}