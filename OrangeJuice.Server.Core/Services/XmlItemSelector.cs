using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlItemSelector : IItemSelector
	{
		private readonly IValidator<XElement> _itemValidator;

		public XmlItemSelector(IValidator<XElement> itemValidator)
		{
			_itemValidator = itemValidator;
		}

		public IEnumerable<XElement> SelectItems(string xml)
		{
			XDocument doc = XDocument.Parse(xml);
			XNamespace ns = doc.Root.Name.Namespace;

			XElement items = doc.Root.Element(ns + "Items");
			if (items == null)
				throw new ArgumentException();
			if (!_itemValidator.IsValid(items))
				throw new ArgumentException(GetErrorMessage(doc, ns));

			return items.Elements(ns + "Item");
		}

		private static string GetErrorMessage(XDocument doc, XNamespace ns)
		{
			var nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", ns.NamespaceName);

			return doc.XPathSelectElement("//x:Error", nm).Value;
		}
	}
}