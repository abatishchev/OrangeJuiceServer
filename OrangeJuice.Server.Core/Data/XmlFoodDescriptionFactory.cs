using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace OrangeJuice.Server.Data
{
	public sealed class XmlFoodDescriptionFactory : IFoodDescriptionFactory
	{
		#region IFoodDescriptionFactory Members
		public string GetId(XElement element)
		{
			if (element == null)
				throw new ArgumentNullException("element");

			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			return (string)element.XPathSelectElement("x:ASIN", nm);
		}

		public FoodDescription Create(XElement attributesElement, XElement imagesElement)
		{
			if (attributesElement == null)
				throw new ArgumentNullException("attributesElement");
			if (imagesElement == null)
				throw new ArgumentNullException("imagesElement");

			return GetSteps(attributesElement, imagesElement).Aggregate(new FoodDescription(), (d, func) => func(d));
		}

		#endregion

		#region Methods
		private static IEnumerable<Func<FoodDescription, FoodDescription>> GetSteps(XElement attributesElement, XElement imagesElement)
		{
			yield return d => AssignAttributes(d, attributesElement);
			yield return d => AssignImages(d, imagesElement);
		}

		internal static FoodDescription AssignAttributes(FoodDescription description, XElement element)
		{
			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			description.Title = (string)element.XPathSelectElement("x:ItemAttributes/x:Title", nm);
			description.Brand = (string)element.XPathSelectElement("x:ItemAttributes/x:Brand", nm);

			return description;
		}

		internal static FoodDescription AssignImages(FoodDescription description, XElement element)
		{
			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			description.SmallImageUrl = (string)element.XPathSelectElement("x:SmallImage/x:URL", nm);
			description.MediumImageUrl = (string)element.XPathSelectElement("x:MediumImage/x:URL", nm);
			description.LargeImageUrl = (string)element.XPathSelectElement("x:LargeImage/x:URL", nm);

			return description;
		}
		#endregion
	}
}