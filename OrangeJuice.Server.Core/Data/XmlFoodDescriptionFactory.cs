using System;
using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public sealed class XmlFoodDescriptionFactory : IFoodDescriptionFactory
	{
		#region IFoodDescriptionFactory Members
		public string GetId(XElement item)
		{
			if (item == null)
				throw new ArgumentNullException("item");
			XNamespace ns = item.Name.Namespace;
			return (string)item.Element(ns + "ASIN");
		}

		public FoodDescription Create(XElement attributesElement, XElement imagesElement)
		{
			if (attributesElement == null)
				throw new ArgumentNullException("attributesElement");
			if (imagesElement == null)
				throw new ArgumentNullException("imagesElement");

			FoodDescription description = new FoodDescription();

			AssignAttributes(description, attributesElement);
			AssignImages(description, imagesElement);

			return description;
		}

		#endregion

		#region Methods
		internal static void AssignAttributes(FoodDescription description, XElement attributesElement)
		{
			XNamespace ns = attributesElement.Name.Namespace;

			description.Title = GetAttribute(attributesElement, ns, "Title");
			description.Brand = GetAttribute(attributesElement, ns, "Brand");
		}

		internal static void AssignImages(FoodDescription description, XElement imagesElement)
		{
			XNamespace ns = imagesElement.Name.Namespace;

			description.SmallImageUrl = GetImageUrl(imagesElement, ns, "SmallImage");
			description.MediumImageUrl = GetImageUrl(imagesElement, ns, "MediumImage");
			description.LargeImageUrl = GetImageUrl(imagesElement, ns, "LargeImage");
		}

		// ReSharper disable PossibleNullReferenceException
		private static string GetAttribute(XContainer attributesElement, XNamespace ns, string elementName)
		{
			return (string)attributesElement.Element(ns + "ItemAttributes")
											.Element(ns + elementName);
		}

		private static string GetImageUrl(XContainer imagesElement, XNamespace ns, string elementName)
		{
			XElement element = imagesElement.Element(ns + elementName);
			return element != null ? (string)element.Element(ns + "URL") : null;
		}
		#endregion

	}
}