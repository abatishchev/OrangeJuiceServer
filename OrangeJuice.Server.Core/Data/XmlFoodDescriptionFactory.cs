using System;
using System.Threading.Tasks;
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

		public async Task<FoodDescription> Create(string id, Task<XElement> attributesTask, Task<XElement> imagesTask)
		{
			if (String.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");
			if (attributesTask == null)
				throw new ArgumentNullException("attributesTask");
			if (imagesTask == null)
				throw new ArgumentNullException("imagesTask");

			FoodDescription description = new FoodDescription();

			AssignId(description, id);

			XElement attributesElement = await attributesTask;
			AssignAttributes(description, attributesElement);

			XElement imagesElement = await imagesTask;
			AssignImages(description, imagesElement);

			return description;
		}
		#endregion

		#region Methods
		private static void AssignId(FoodDescription description, string id)
		{
			description.ASIN = id;
		}

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