using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public sealed class FoodDescriptionFactory : IFoodDescriptionFactory
	{
		public async Task<FoodDescription> Create(string id, Task<XElement> attributesTask, Task<XElement> imagesTask)
		{
			return await NewFlow(id, attributesTask, imagesTask).Aggregate(
				Task.FromResult(new FoodDescription()),
				(d, func) => func(d));
		}

		private static IEnumerable<Func<Task<FoodDescription>, Task<FoodDescription>>> NewFlow(string id, Task<XElement> attributesTask, Task<XElement> imagesTask)
		{
			yield return d => AssignId(d, id);
			yield return d => ParseAttributes(d, attributesTask);
			yield return d => ParseImages(d, imagesTask);
		}

		private static async Task<FoodDescription> AssignId(Task<FoodDescription> descriptionTask, string id)
		{
			FoodDescription description = await descriptionTask;
			description.ASIN = id;
			return description;
		}

		//TODO: test
		private static async Task<FoodDescription> ParseAttributes(Task<FoodDescription> descriptionTask, Task<XElement> attributesTask)
		{
			FoodDescription description = await descriptionTask;
			XElement attributesElement = await attributesTask;
			XNamespace ns = attributesElement.Name.Namespace;

			description.Title = GetAttribute(attributesElement, ns, "Title");
			description.Brand = GetAttribute(attributesElement, ns, "Brand");

			return description;
		}

		//TODO: test
		private static async Task<FoodDescription> ParseImages(Task<FoodDescription> descriptionTask, Task<XElement> imagesTask)
		{
			FoodDescription description = await descriptionTask;
			XElement imagesElement = await imagesTask;
			XNamespace ns = imagesElement.Name.Namespace;

			description.SmallImageUrl = GetImageUrl(imagesElement, ns, "SmallImage");
			description.MediumImageUrl = GetImageUrl(imagesElement, ns, "MediumImage");
			description.LargeImageUrl = GetImageUrl(imagesElement, ns, "LargeImage");

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