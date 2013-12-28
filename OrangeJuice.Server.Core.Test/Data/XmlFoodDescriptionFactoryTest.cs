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
		public void GetId_Should_Throw_Exception_When_Element_Is_Null()
		{
			// Arrange
			const XElement element = null;

			XmlFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			Action action = () => factory.GetId(element);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("element");
		}

		[TestMethod]
		public void GetId_Should_Return_Asin_From_Element()
		{
			// Arrange
			const string expected = "ASIN";
			const string ns = "ns";

			XElement element = new XElement(XName.Get("anyElement", ns),
				new XElement(XName.Get("ASIN", ns),
					expected));

			XmlFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			string actual = factory.GetId(element);

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void Create_Should_Throw_Exception_When_AttributesElement_Is_Null()
		{
			// Arrange
			const XElement attributesElement = null;
			const XElement imagesElement = null;

			XmlFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			Action action = () => factory.Create(attributesElement, imagesElement);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("attributesElement");
		}

		[TestMethod]
		public void Create_Should_Throw_Exception_When_ImagesElement_Is_Null()
		{
			// Arrange
			XElement attributesElement = new XElement("attributesElement");
			const XElement imagesElement = null;

			XmlFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			Action action = () => factory.Create(attributesElement, imagesElement);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("imagesElement");
		}

		[TestMethod]
		public void Create_Should_Return_GroceryDescription_Having_Properies_Assigned_From_AttributesElement()
		{
			// Arrange
			const string title = "title";
			const string brand = "brand";

			XElement attributesTask = CreateAttributes(title, brand);
			XElement imagesTask = CreateImages();

			XmlFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			FoodDescription description = factory.Create(attributesTask, imagesTask);

			// Assert
			description.Title.Should().Be(title);
			description.Brand.Should().Be(brand);
		}

		[TestMethod]
		public void Create_Should_Return_GroceryDescription_Having_Properies_Assigned_From_ImagesElement()
		{
			// Arrange
			const string smallImageUrl = "smallImageUrl";
			const string mediumImageUrl = "mediumImageUrl";
			const string largeImageUrl = "largeImageUrl";

			XElement attributesTask = CreateAttributes();
			XElement imagesTask = CreateImages(smallImageUrl, mediumImageUrl, largeImageUrl);

			XmlFoodDescriptionFactory factory = new XmlFoodDescriptionFactory();

			// Act
			FoodDescription description = factory.Create(attributesTask, imagesTask);

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