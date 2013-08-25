using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public sealed class FoodDescriptionFactory : IFoodDescriptionFactory
	{
		public FoodDescription Create(Task<XElement> attributesTask, Task<XElement> imagesTask)
		{
			FoodDescription description = new FoodDescription();

			attributesTask = attributesTask.ContinueWith(t =>
				{
					XElement itemElement = t.Result;
					XNamespace ns = itemElement.Name.Namespace;
					XElement attributesElement = itemElement.Element(ns + "ItemAttributes");

					description.ASIN = GetAttribute(itemElement, ns, "ASIN");
					description.Title = GetAttribute(attributesElement, ns, "Title");
					description.Brand = GetAttribute(attributesElement, ns, "Brand");

					return t.Result;
				});

			imagesTask = imagesTask.ContinueWith(t =>
				{
					XElement imagesElement = t.Result;
					XNamespace ns = imagesElement.Name.Namespace;

					description.SmallImageUrl = GetImageUrl(imagesElement, ns, "SmallImage");
					description.MediumImageUrl = GetImageUrl(imagesElement, ns, "MediumImage");
					description.LargeImageUrl = GetImageUrl(imagesElement, ns, "LargeImage");

					return t.Result;
				});

			Task.WaitAll(attributesTask, imagesTask);
			return description;
		}

		private static string GetAttribute(XElement element, XNamespace ns, string elementName)
		{
			return (string)element.Element(ns + elementName);
		}

		private static string GetImageUrl(XElement element, XNamespace ns, string elementName)
		{
			// ReSharper disable once PossibleNullReferenceException
			return (string)element.Element(ns + elementName)
								  .Element(ns + "URL");
		}
	}
}