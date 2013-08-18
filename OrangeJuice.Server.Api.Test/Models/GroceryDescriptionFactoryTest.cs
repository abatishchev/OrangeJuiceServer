using System;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Test.Models
{
	[TestClass]
	public class GroceryDescriptionFactoryTest
	{
		[TestMethod]
		public void Create_Should_Populate_GroceryDescription_Properies_From_XElement()
		{
			// Arrange
			const string asin = "asin";
			const string title = "title";
			const string detailPageUrl = "detailPageUrl";
			const string technicalUrl = "technicalDetailsUrl";

			XElement element = CreateElement(asin, title, detailPageUrl, technicalUrl);
			GroceryDescriptionFactory factory = new GroceryDescriptionFactory();

			// Act
			GroceryDescription description = factory.Create(element);

			// Assert
			description.ASIN.Should().Be(asin);
			description.Title.Should().Be(title);
			description.DetailPageUrl.Should().Be(detailPageUrl);
			description.TechnicalDetailsUrl.Should().Be(technicalUrl);
		}

		private static XElement CreateElement(string asin, string title, string detailPageUrl, string technicalDetailsUrl)
		{
			const string xml = @"<Item xmlns=""http://webservices.amazon.com/AWSECommerceService/latest"">
  <ASIN>{0}</ASIN>
  <DetailPageURL>{2}</DetailPageURL>
  <ItemLinks>
    <ItemLink>
      <Description>Technical Details</Description>
      <URL>{3}</URL>
    </ItemLink>
  </ItemLinks>
  <ItemAttributes>
    <ProductGroup>Grocery</ProductGroup>
    <Title>{1}</Title>
  </ItemAttributes>
</Item>";
			return XElement.Parse(String.Format(xml, asin, title, detailPageUrl, technicalDetailsUrl));
		}
	}
}