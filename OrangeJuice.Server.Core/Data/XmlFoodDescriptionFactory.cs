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
			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			return (string)element.XPathSelectElement("x:ASIN", nm);
		}

		public FoodDescription Create(XElement attributesElement, XElement imagesElement)
		{
			FoodDescription description = new FoodDescription();
			AssignAttributes(description, attributesElement);
			AssignImages(description, imagesElement);
			return description;
		}
		#endregion

		#region Methods
		internal static void AssignAttributes(FoodDescription description, XElement element)
		{
			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			description.Title = (string)element.XPathSelectElement("x:ItemAttributes/x:Title", nm);
			description.Brand = (string)element.XPathSelectElement("x:ItemAttributes/x:Brand", nm);
		}

		internal static void AssignImages(FoodDescription description, XElement element)
		{
			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			description.SmallImageUrl = (string)element.XPathSelectElement("x:SmallImage/x:URL", nm);
			description.MediumImageUrl = (string)element.XPathSelectElement("x:MediumImage/x:URL", nm);
			description.LargeImageUrl = (string)element.XPathSelectElement("x:LargeImage/x:URL", nm);
		}
		#endregion
	}
}