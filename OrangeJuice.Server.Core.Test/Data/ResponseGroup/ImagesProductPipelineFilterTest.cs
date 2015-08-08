using FluentAssertions;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Data.ResponseGroup;

using Xunit;

namespace OrangeJuice.Server.Test.Data.ResponseGroup
{
	public class ImagesProductPipelineFilterTest
	{
		[Fact]
		public void Execute_Should_Return_ProductDescriptor_Having_Images_When_ResponseGroups_Contains_Images()
		{
			// Arrange
			const string smallImageUrl = "smallImageUrl";
			const string mediumImageUrl = "mediumImageUrl";
			const string largeImageUrl = "largeImageUrl";

			var element = XElementFactory.Create(smallImageUrl: smallImageUrl, mediumImageUrl: mediumImageUrl, largeImageUrl: largeImageUrl);

			var searchCriteria = new AwsProductSearchCriteria
			{
				ResponseGroups = new[] { "Images" }
			};

			var filter = new ImagesProductPipelineFilter();

			// Act
			var descriptor = filter.Execute(new ProductDescriptor(), element, searchCriteria);

			// Assert
			descriptor.SmallImageUrl.Should().Be(smallImageUrl);
			descriptor.MediumImageUrl.Should().Be(mediumImageUrl);
			descriptor.LargeImageUrl.Should().Be(largeImageUrl);
		}
	}
}