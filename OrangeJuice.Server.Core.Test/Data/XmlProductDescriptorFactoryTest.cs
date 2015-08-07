using System;
using System.Xml.Linq;

using Factory;
using FluentAssertions;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;

using Xunit;

namespace OrangeJuice.Server.Test.Data
{
	public class XmlProductDescriptorFactoryTest
	{
		#region Test methods

		[Theory]
		[InlineData(typeof(XmlProductDescriptorFactory))]
		[InlineData(typeof(FSharp.Data.XmlProductDescriptorFactory))]
		public void Create_Should_Return_ProductDescriptor_Having_BaseAttributes(Type type)
		{
			// Arrange
			const string asin = "asin";
			Uri detailPageUrl = new Uri("http://www.amazon.com/name/dp/asin");

			XElement element = CreateElement(asin, detailPageUrl: detailPageUrl);

			var factory = CreateFactory(type);

			// Act
			ProductDescriptor descriptor = factory.Create(element);

			// Assert
			descriptor.SourceProductId.Should().Be(asin);
			descriptor.DetailPageUrl.Should().Be(detailPageUrl);
		}

		[Theory]
		[InlineData(typeof(XmlProductDescriptorFactory))]
		[InlineData(typeof(FSharp.Data.XmlProductDescriptorFactory))]
		public void Create_Should_Return_ProductDescriptor_Having_Attributes(Type type)
		{
			// Arrange
			const string title = "title";
			const string brand = "brand";

			XElement element = CreateElement(title: title, brand: brand);

			var factory = CreateFactory(type);

			// Act
			ProductDescriptor descriptor = factory.Create(element);

			// Assert
			descriptor.Title.Should().Be(title);
			descriptor.Brand.Should().Be(brand);
		}

		[Theory]
		[InlineData(typeof(XmlProductDescriptorFactory))]
		[InlineData(typeof(FSharp.Data.XmlProductDescriptorFactory))]
		public void Create_Should_Return_ProductDescriptor_Having_Images(Type type)
		{
			// Arrange
			const string smallImageUrl = "smallImageUrl";
			const string mediumImageUrl = "mediumImageUrl";
			const string largeImageUrl = "largeImageUrl";

			XElement element = CreateElement(smallImageUrl: smallImageUrl, mediumImageUrl: mediumImageUrl, largeImageUrl: largeImageUrl);

			var factory = CreateFactory(type);

			// Act
			ProductDescriptor descriptor = factory.Create(element);

			// Assert
			descriptor.SmallImageUrl.Should().Be(smallImageUrl);
			descriptor.MediumImageUrl.Should().Be(mediumImageUrl);
			descriptor.LargeImageUrl.Should().Be(largeImageUrl);
		}

		[Theory]
		[InlineData(typeof(XmlProductDescriptorFactory))]
		[InlineData(typeof(FSharp.Data.XmlProductDescriptorFactory))]
		public void Create_Should_Return_ProductDescriptor_Having_LowestNewPrice(Type type)
		{
			// Arrange
			const float lowestNewPrice = 2.5f;

			XElement element = CreateElement(lowestNewPrice: lowestNewPrice);

			var factory = CreateFactory(type);

			// Act
			ProductDescriptor descriptor = factory.Create(element);

			// Assert
			descriptor.LowestNewPrice.Should().Be(lowestNewPrice);
		}
		#endregion

		#region Helper methods
		private static IFactory<ProductDescriptor, XElement> CreateFactory(Type type)
		{
			return (IFactory<ProductDescriptor, XElement>)Activator.CreateInstance(type);
		}

		private static XElement CreateElement(string asin = "",
											  string title = "", string brand = "", Uri detailPageUrl = null,
											  string smallImageUrl = "", string mediumImageUrl = "", string largeImageUrl = "",

											  float lowestNewPrice = 0)
		{
			XNamespace ns = "http://webservices.amazon.com/AWSECommerceService/latest";
			return new XElement(ns + "Item",
				new XElement(ns + "ASIN", asin),
				new XElement(ns + "DetailPageURL", detailPageUrl ?? new Uri(ns.ToString())),

				new XElement(ns + "SmallImage",
					new XElement(ns + "URL", smallImageUrl)),
				new XElement(ns + "MediumImage",
					new XElement(ns + "URL", mediumImageUrl)),
				new XElement(ns + "LargeImage",
					new XElement(ns + "URL", largeImageUrl)),

				new XElement(ns + "ItemAttributes",
					new XElement(ns + "Title", title),
					new XElement(ns + "Brand", brand)),

				new XElement(ns + "OfferSummary",
					new XElement(ns + "LowestNewPrice",
						new XElement(ns + "Amount", lowestNewPrice))));
		}

		#endregion
	}
}