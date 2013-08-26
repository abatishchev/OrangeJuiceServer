using System;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Test.Data
{
	[TestClass]
	public class XmlFoodDescriptionFactoryTest
	{
		[TestMethod]
		public async Task Create_Should_Return_GroceryDescription_Having_Properies_Populated_From_XElement()
		{
			// Arrange
			const string asin = "asin";
			var attributesTask = Task.Factory.StartNew(() => CreateAttributes());
			var imagesTask = Task.Factory.StartNew(() => CreateImages());

			XmlFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			FoodDescription description = await factory.Create(asin, attributesTask, imagesTask);

			// Assert
			description.ASIN.Should().NotBeNull();
			description.Title.Should().NotBeNull();
			description.Brand.Should().NotBeNull();

			description.SmallImageUrl.Should().NotBeNull();
			description.MediumImageUrl.Should().NotBeNull();
			description.LargeImageUrl.Should().NotBeNull();
		}

		[TestMethod]
		public void AssignAttributes_Should_Assign_Attributes()
		{
			// Arrange
			const string title = "title";
			const string brand = "brand";

			FoodDescription description = new FoodDescription();
			XElement attributesElement = CreateAttributes(title, brand);

			// Act
			XmlFoodDescriptionFactory.AssignAttributes(description, attributesElement);

			// Assert
			description.Title.Should().Be(title);
			description.Brand.Should().Be(brand);
		}

		[TestMethod]
		public void AssignImages_Should_Assign_Images()
		{
			// Arrange
			const string smallImageUrl = "smallImageUrl";
			const string mediumImageUrl = "mediumImageUrl";
			const string largeImageUrl = "largeImageUrl";

			FoodDescription description = new FoodDescription();
			XElement imagesElement = CreateImages(smallImageUrl, mediumImageUrl, largeImageUrl);

			// Act
			XmlFoodDescriptionFactory.AssignImages(description, imagesElement);

			// Assert
			description.SmallImageUrl.Should().Be(smallImageUrl);
			description.MediumImageUrl.Should().Be(mediumImageUrl);
			description.LargeImageUrl.Should().Be(largeImageUrl);
		}

		private static XElement CreateAttributes(string title = "", string brand = "")
		{
			return XElement.Parse(String.Format(
@"<Item xmlns=""http://webservices.amazon.com/AWSECommerceService/latest"">
	<ItemAttributes>
		<Title>{0}</Title>
		<Brand>{1}</Brand>
	</ItemAttributes>
</Item>", title, brand));
		}

		private static XElement CreateImages(string smallImageUrl = "", string mediumImageUrl = "", string largeImageUrl = "")
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