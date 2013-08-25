using System;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Test.Data
{
	[TestClass]
	public class FoodDescriptionFactoryTest
	{
		[TestMethod]
		public void Create_Should_Return_GroceryDescription_Having_Properies_Populated_From_XElement()
		{
			// Arrange
			const string asin = "asin";
			const string title = "title";
			const string brand = "brand";
			var attributeTask = Task.FromResult(CreateAttributes(asin, title, brand));

			const string smallImageUrl = "smallImageUrl";
			const string mediumImageUrl = "mediumImageUrl";
			const string largeImageUrl = "largeImageUrl";
			var imagesTask = Task.FromResult(CreateImages(smallImageUrl, mediumImageUrl, largeImageUrl));

			FoodDescriptionFactory factory = new FoodDescriptionFactory();

			// Act
			FoodDescription description = factory.Create(attributeTask, imagesTask);

			// Assert
			description.ASIN.Should().Be(asin);
			description.Title.Should().Be(title);

			description.SmallImageUrl.Should().Be(smallImageUrl);
			description.MediumImageUrl.Should().Be(mediumImageUrl);
			description.LargeImageUrl.Should().Be(largeImageUrl);
		}

		private static XElement CreateAttributes(string asin, string title, string brand)
		{
			return XElement.Parse(String.Format(
@"<Item xmlns=""http://webservices.amazon.com/AWSECommerceService/latest"">
	<ASIN>{0}</ASIN>
	<ItemAttributes>
		<Title>{1}</Title>
		<Brand>{2}</Brand>
	</ItemAttributes>
</Item>", asin, title, brand));
		}

		private static XElement CreateImages(string smallImageUrl, string mediumImageUrl, string largeImageUrl)
		{
			return XElement.Parse(String.Format(
@"<Item xmlns=""http://webservices.amazon.com/AWSECommerceService/latest"">
	<SmallImage>
		<URL>{0}</URL>
	</SmallImage>
	<MediumImage>
		<URL>{1}</URL>
	</MediumImage>
	<LargeImage>
		<URL>{2}</URL>
	</LargeImage>
</Item>", smallImageUrl, mediumImageUrl, largeImageUrl));
		}
	}
}