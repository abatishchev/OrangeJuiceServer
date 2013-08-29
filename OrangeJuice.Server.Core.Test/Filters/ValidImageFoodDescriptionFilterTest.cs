using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Filters;

namespace OrangeJuice.Server.Test.Filters
{
	[TestClass]
	public class ValidImageFoodDescriptionFilterTest
	{
		[TestMethod]
		public void Filter_Should_Throw_Exception_When_FoodDescription_Is_Null()
		{
			// Arrange
			const FoodDescription foodDescription = null;
			IFilter<FoodDescription> filter = new ValidImageFoodDescriptionFilter();

			// Act
			Action action = () => filter.Filter(foodDescription);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("foodDescription");
		}

		[TestMethod]
		public void Filter_Should_Return_True_When_All_ImageUrls_Are_Not_Null()
		{
			// Arrange
			FoodDescription foodDescription = new FoodDescription
			{
				LargeImageUrl = "anyUrl",
				MediumImageUrl = "anyUrl",
				SmallImageUrl = "anyUrl"
			};
			IFilter<FoodDescription> filter = new ValidImageFoodDescriptionFilter();

			// Act
			bool result = filter.Filter(foodDescription);

			// Assert
			result.Should().BeTrue();
		}

		[TestMethod]
		public void Filter_Should_Return_False_When_LargeImageUrl_Is_Null()
		{
			// Arrange
			FoodDescription foodDescription = new FoodDescription
			{
				LargeImageUrl = null,
				MediumImageUrl = "anyUrl",
				SmallImageUrl = "anyUrl"
			};
			IFilter<FoodDescription> filter = new ValidImageFoodDescriptionFilter();

			// Act
			bool result = filter.Filter(foodDescription);

			// Assert
			result.Should().BeFalse();
		}

		[TestMethod]
		public void Filter_Should_Return_False_When_MediumImageUrl_Is_Null()
		{
			// Arrange
			FoodDescription foodDescription = new FoodDescription
			{
				LargeImageUrl = "anyUrl",
				MediumImageUrl = null,
				SmallImageUrl = "anyUrl"
			};
			IFilter<FoodDescription> filter = new ValidImageFoodDescriptionFilter();

			// Act
			bool result = filter.Filter(foodDescription);

			// Assert
			result.Should().BeFalse();
		}

		[TestMethod]
		public void Filter_Should_Return_False_When_SmallImageUrl_Is_Null()
		{
			// Arrange
			FoodDescription foodDescription = new FoodDescription
			{
				LargeImageUrl = "anyUrl",
				MediumImageUrl = "anyUrl",
				SmallImageUrl = null
			};
			IFilter<FoodDescription> filter = new ValidImageFoodDescriptionFilter();

			// Act
			bool result = filter.Filter(foodDescription);

			// Assert
			result.Should().BeFalse();
		}
	}
}