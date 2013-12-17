﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Test;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	[TestClass]
	public class FoodControllerTest
	{
		#region Ctor
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AwsFoodRepository_Is_Null()
		{
			// Arrange
			const AwsFoodRepository foodRepository = null;

			// Act
			Action action = () => new FoodController(foodRepository);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("foodRepository");
		}
		#endregion

		#region GetDescription
		[TestMethod]
		public async Task GetDescription_Should_Return_BadRequest_When_SearchCriteria_Is_Null()
		{
			// Arrange
			FoodController controller = CreateController();
			const FoodSearchCriteria searchCriteria = null;

			// Act
			IHttpActionResult result = await controller.GetDescription(searchCriteria);

			// Assert
			result.Should().BeOfType<BadRequestErrorMessageResult>();
		}

		[TestMethod]
		public async Task GetDescription_Should_Return_BadRequest_When_Model_Not_IsValid()
		{
			// Arrange
			FoodController controller = CreateController(exception: new ArgumentNullException());
			FoodSearchCriteria searchCriteria = new FoodSearchCriteria();

			// Act
			IHttpActionResult result  = await controller.GetDescription(searchCriteria);

			// Assert
			result.Should().BeOfType<BadRequestErrorMessageResult>();
		}

		[TestMethod]
		public async Task GetDescription_Should_Pass_Title_To_FoodRepository_SearchByTitle()
		{
			// Arrange
			const string title = "title";

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.SearchByTitle(title)).ReturnsAsync(new[] { new FoodDescription() });

			FoodController controller = CreateController(foodRepositoryMock.Object);
			FoodSearchCriteria searchCriteria = new FoodSearchCriteria { Title = title };

			// Act
			await controller.GetDescription(searchCriteria);

			// Assert
			foodRepositoryMock.Verify(r => r.SearchByTitle(title), Times.Once());
		}

		[TestMethod]
		public async Task GetDescription_Should_Return_Description_Of_Found_Food()
		{
			// Arrange
			FoodDescription[] expected = { new FoodDescription() };

			var foodRepositoryMock = new Mock<IFoodRepository>();
			foodRepositoryMock.Setup(r => r.SearchByTitle(It.IsAny<string>())).ReturnsAsync(expected);

			FoodController controller = CreateController(foodRepositoryMock.Object);
			FoodSearchCriteria searchCriteria = new FoodSearchCriteria();

			// Act
			var result  = await controller.GetDescription(searchCriteria) as OkNegotiatedContentResult<FoodDescription[]>;
			FoodDescription[] actual = result.Content;

			// Assert
			actual.ShouldBeEquivalentTo(expected);
		}
		#endregion

		#region Helper methods
		private static FoodController CreateController(IFoodRepository repository = null, Exception exception = null)
		{
			var controller = ControllerFactory.Create<FoodController>(repository ?? new Mock<IFoodRepository>(MockBehavior.Strict).Object);
			if (exception != null)
				controller.ModelState.AddModelError("", exception);
			return controller;
		}
		#endregion
	}
}