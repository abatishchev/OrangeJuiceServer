using FluentAssertions;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Data.ResponseGroup;

using Xunit;

namespace OrangeJuice.Server.Test.Data.ResponseGroup
{
	public class OfferSummaryPipelineFilterTest
	{
		[Fact]
		public void Execute_Should_Return_ProductDescriptor_Having_OfferSummary_When_ResponseGroups_Contains_OfferSummary()
		{
			// Arrange
			const float lowestNewPrice = 2.5f;

			var element = XElementFactory.Create(lowestNewPrice: lowestNewPrice);

			var searchCriteria = new AwsProductSearchCriteria
			{
				ResponseGroups = new[] { "OfferSummary" }
			};

			var filter = new OfferSummaryPipelineFilter();

			// Act
			var descriptor = filter.Execute(new ProductDescriptor(), element, searchCriteria);

			// Assert
			descriptor.LowestNewPrice.Should().Be(lowestNewPrice);
		}
	}
}