using System;
using System.Linq;
using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public sealed class FoodDescriptionFactory : IFoodDescriptionFactory
	{
		public FoodDescription Create(XElement element)
		{
			XNamespace ns = element.Name.Namespace;
			XElement itemAttributes = element.Element(ns + "ItemAttributes");

			return new FoodDescription
			{
				ASIN = (string)element.Element(ns + "ASIN"),
				Title = (string)itemAttributes.Element(ns + "Title"),
				Manufacturer = (string)itemAttributes.Element(ns + "Manufacturer"),

				DetailPageUrl = (string)element.Element(ns + "DetailPageURL"),
				TechnicalDetailsUrl = (string)element.Elements(ns + "ItemLinks")
													 .Elements(ns + "ItemLink")
													 .Single(e => (string)e.Element(ns + "Description") == "Technical Details")
													 .Element(ns + "URL")
			};
		}
	}
}