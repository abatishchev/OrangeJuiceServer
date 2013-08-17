using System;
using System.Linq;
using System.Xml.Linq;

namespace OrangeJuice.Server.Api.Models
{
	public sealed class GroceryDescriptionFactory
	{
		public GroceryDescription Create(XElement element)
		{
			XNamespace ns = element.Name.Namespace;
			XElement itemAttributes = element.Element(ns + "ItemAttributes");

			return new GroceryDescription
			{
				ASIN = (string)element.Element(ns + "ASIN"),
				Title = (string)itemAttributes.Element(ns + "Title"),
				Manufacturer = (string)itemAttributes.Element(ns + "Manufacturer"),

				DetailPageUrl = (string)element.Element(ns + "DetailPageUrl"),
				TechnicalDetailsUrl = (string)element.Elements(ns + "ItemLink")
													 .Single(e => (string)e.Element(ns + "Description") == "Technical Details")
													 .Element(ns + "URL"),
			};
		}
	}
}