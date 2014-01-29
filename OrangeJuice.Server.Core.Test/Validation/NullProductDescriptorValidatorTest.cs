﻿using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Validation;

namespace OrangeJuice.Server.Test.Validation
{
	[TestClass]
	public class NullProductDescriptorValidatorTest
	{
		[TestMethod]
		public void Validate_Should_Return_True_When_All_Properies_Are_Not_Null()
		{
			// Arrange
			ProductDescriptor descriptor = new ProductDescriptor
			{
				Id = "id",
				Title = "title",
				Brand = "brand",
				SmallImageUrl = "smallImageUrl",
				MediumImageUrl = "mediumImageUrl",
				LargeImageUrl = "largeImageUrl"
			};
			IValidator<ProductDescriptor> validator = new NullProductDescriptorValidator();

			// Act
			bool isValid = validator.IsValid(descriptor);

			// Assert
			isValid.Should().BeTrue();
		}
	}
}