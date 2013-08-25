﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Controllers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Data;

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
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			HttpResponseMessage message = await controller.GetDescription(searchCriteria);
			HttpStatusCode actual = message.StatusCode;

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public async Task GetDescription_Should_Return_BadRequest_When_Model_Not_IsValid()
		{
			// Arrange
			FoodController controller = CreateController();
			FoodSearchCriteria searchCriteria = new FoodSearchCriteria();
			const HttpStatusCode expected = HttpStatusCode.BadRequest;

			// Act
			using (ControllerFactory.NewContext(ControllerFactory.CreateModelValidator(s => false)))
			{
				HttpResponseMessage message = await controller.GetDescription(searchCriteria);
				HttpStatusCode actual = message.StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}

		[TestMethod]
		public async Task GetDescription_Should_Pass_SearchCriteria_To_FoodRepository_Register()
		{
			// Arrange
			Assert.Inconclusive();

			FoodController controller = CreateController();
			FoodSearchCriteria searchCriteria = new FoodSearchCriteria();

			// Act
			using (ControllerFactory.NewContext())
			{
				await controller.GetDescription(searchCriteria);

				// Assert
				//userRepositoryMock.Verify(r => r.Register(email), Times.Once());
			}
		}

		[TestMethod]
		public async Task GetDescription_Should_Return_Description_Of_Found_Food()
		{
			// Arrange
			FoodDescription expected = new FoodDescription();

			Assert.Inconclusive();

			FoodController controller = CreateController();
			FoodSearchCriteria searchCriteria = new FoodSearchCriteria();

			// Act
			using (ControllerFactory.NewContext())
			{
				HttpResponseMessage message = await controller.GetDescription(searchCriteria);
				HttpStatusCode actual = message.StatusCode;

				// Assert
				actual.Should().Be(expected);
			}
		}
		#endregion

		#region Helper methods
		private static FoodController CreateController(IFoodRepository repository = null)
		{
			return ControllerFactory.Create<FoodController>(repository ?? new Mock<IFoodRepository>(MockBehavior.Strict).Object);
		}
		#endregion
	}
}