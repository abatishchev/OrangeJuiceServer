using System;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Test.Data
{
	[TestClass]
	public class XmlFoodDescriptionFactoryTest
	{
		#region Test methods
		[TestMethod]
		public void Create_Should_Return_FoodDescription_Having_Id()
		{
			// Arrange
			const string id = "id";

			XElement element = CreateElement(id);

			IFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			FoodDescription description = factory.Create(element);

			// Assert
			description.Id.Should().Be(id);
		}

		[TestMethod]
		public void Create_Should_Return_FoodDescription_Having_Attributes()
		{
			// Arrange
			const string title = "title";
			const string brand = "brand";

			XElement element = CreateElement(title: title, brand: brand);

			IFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			FoodDescription description = factory.Create(element);

			// Assert
			description.Title.Should().Be(title);
			description.Brand.Should().Be(brand);
		}

		[TestMethod]
		public void Create_Should_Return_FoodDescription_Having_Images()
		{
			// Arrange
			const string smallImageUrl = "smallImageUrl";
			const string mediumImageUrl = "mediumImageUrl";
			const string largeImageUrl = "largeImageUrl";

			XElement element = CreateElement(smallImageUrl: smallImageUrl, mediumImageUrl: mediumImageUrl, largeImageUrl: largeImageUrl);

			IFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			FoodDescription description = factory.Create(element);

			// Assert
			description.SmallImageUrl.Should().Be(smallImageUrl);
			description.MediumImageUrl.Should().Be(mediumImageUrl);
			description.LargeImageUrl.Should().Be(largeImageUrl);
		}
		#endregion

		#region Helper methods
		private static XElement CreateElement(string id = "", string smallImageUrl = "", string mediumImageUrl = "", string largeImageUrl = "", string title = "", string brand = "")
		{
			return XElement.Parse(String.Format(
@"<Item xmlns=""http://webservices.amazon.com/AWSECommerceService/latest"">
	<ASIN>{0}</ASIN>
	<SmallImage>
		<URL>{1}</URL>
	</SmallImage>
	<MediumImage>
		<URL>{2}</URL>
	</MediumImage>
	<LargeImage>
		<URL>{3}</URL>
	</LargeImage>
	<ItemAttributes>
		<Title>{4}</Title>
		<Brand>{5}</Brand>
	</ItemAttributes>
</Item>",
		id,
		smallImageUrl, mediumImageUrl, largeImageUrl,
		title, brand));
		}
		#endregion
	}
}