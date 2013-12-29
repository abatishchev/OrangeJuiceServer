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
		public void Create_Should_Return_GroceryDescription_Having_Id_Assigned_From_Id()
		{
			// Arrange
			const string id = "id";

			XElement attributesTask = CreateAttributes();
			XElement imagesTask = CreateImages();

			XmlFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			FoodDescription description = factory.Create(id, attributesTask, imagesTask);

			// Assert
			description.Id.Should().Be(id);
		}

		[TestMethod]
		public void Create_Should_Return_GroceryDescription_Having_Properies_Assigned_From_AttributesElement()
		{
			// Arrange
			const string id = "id";
			const string title = "title";
			const string brand = "brand";

			XElement attributesTask = CreateAttributes(title, brand);
			XElement imagesTask = CreateImages();

			XmlFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			FoodDescription description = factory.Create(id, attributesTask, imagesTask);

			// Assert
			description.Title.Should().Be(title);
			description.Brand.Should().Be(brand);
		}

		[TestMethod]
		public void Create_Should_Return_GroceryDescription_Having_Properies_Assigned_From_ImagesElement()
		{
			// Arrange
			const string id = "id";
			const string smallImageUrl = "smallImageUrl";
			const string mediumImageUrl = "mediumImageUrl";
			const string largeImageUrl = "largeImageUrl";

			XElement attributesTask = CreateAttributes();
			XElement imagesTask = CreateImages(smallImageUrl, mediumImageUrl, largeImageUrl);

			XmlFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			FoodDescription description = factory.Create(id, attributesTask, imagesTask);

			// Assert
			description.SmallImageUrl.Should().Be(smallImageUrl);
			description.MediumImageUrl.Should().Be(mediumImageUrl);
			description.LargeImageUrl.Should().Be(largeImageUrl);
		}
		#endregion

		#region Helper methods
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
		#endregion
	}
}