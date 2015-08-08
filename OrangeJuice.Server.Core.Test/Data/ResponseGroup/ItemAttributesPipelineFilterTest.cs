using System;

using FluentAssertions;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Data.ResponseGroup;

using Xunit;

namespace OrangeJuice.Server.Test.Data.ResponseGroup
{
	public class ItemAttributesPipelineFilterTest
	{
		[Fact]
		public void Execute_Should_Return_ProductDescriptor_Having_ItemAttributes_When_ResponseGroups_Contains_ItemAttributes()
		{
			// Arrange
			const string brand = "brand";
			var customerReviewsUrl = new Uri("http://www.amazon.com/name/dp/asin");

			var element = XElementFactory.Create(brand: brand, customerReviewsUrl: customerReviewsUrl);

			var searchCriteria = new AwsProductSearchCriteria
			{
				ResponseGroups = new[] { "ItemAttributes" }
			};

			var filter = new ItemAttributesPipelineFilter();

			// Act
			var descriptor = filter.Execute(new ProductDescriptor(), element, searchCriteria);

			// Assert
			descriptor.Brand.Should().Be(brand);
			descriptor.CustomerReviewsUrl.Should().Be(customerReviewsUrl);
		}
	}
}