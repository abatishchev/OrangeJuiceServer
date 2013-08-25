using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public sealed class FoodDescriptionFactory : IFoodDescriptionFactory
	{
		public FoodDescription Create(Task<XElement> descriptionTask, Task<XElement> imageTask)
		{
			FoodDescription description = new FoodDescription();

			descriptionTask = descriptionTask.ContinueWith(t =>
				{
					if (t.Exception != null)
						Debug.Print(t.Exception.ToString());

					XElement descriptionElement = t.Result;
					XNamespace ns = descriptionElement.Name.Namespace;
					XElement itemAttributes = descriptionElement.Element(ns + "ItemAttributes");

					description.ASIN = (string)descriptionElement.Element(ns + "ASIN");
					description.Title = (string)itemAttributes.Element(ns + "Title");
					description.Brand = (string)itemAttributes.Element(ns + "Brand");

					return t.Result;
				});

			imageTask = imageTask.ContinueWith(t =>
				{
					if (t.Exception != null)
						Debug.Print(t.Exception.ToString());

					XElement imageElement = t.Result;
					XNamespace ns = imageElement.Name.Namespace;

					description.SmallImageUrl = (string)imageElement.Element(ns + "SmallImage").Element(ns + "URL");
					description.MediumImageUrl = (string)imageElement.Element(ns + "MediumImage").Element(ns + "URL");
					description.LargeImageUrl = (string)imageElement.Element(ns + "LargeImage").Element(ns + "URL");

					return t.Result;
				});

			Task.WaitAll(descriptionTask, imageTask);
			return description;
		}

		private static string GetValue<T>(XElement descriptionElement, XNamespace ns, Func<FoodDescription, T> propertyFunc)
		{
			return null;
		}
	}
}