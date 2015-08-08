using System;

using FluentAssertions;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Data.ResponseGroup;

using Xunit;

namespace OrangeJuice.Server.Test.Data.ResponseGroup
{
	public class DefaultPipelineFilterTest
	{
		[Fact]
		public void Execute_Should_Return_ProductDescriptor_Having_DefaultAttributes()
		{
			// Arrange
			const string asin = "asin";
			var detailsPageUrl = new Uri("http://www.amazon.com/name/dp/asin");
			const string title = "title";
			const string productGroup = "productGroup";

			var element = XElementFactory.Create(asin, detailsPageUrl: detailsPageUrl, title: title, productGroup: productGroup);

			var filter = new DefaultPipelineFilter();

			// Act
			var descriptor = filter.Execute(new ProductDescriptor(), element, new AwsProductSearchCriteria());

			// Assert
			descriptor.SourceProductId.Should().Be(asin);
			descriptor.DetailsPageUrl.Should().Be(detailsPageUrl);
			descriptor.Title.Should().Be(title);
			descriptor.ProductGroup.Should().Be(productGroup);
		}
	}
}