using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlItemSelector : IItemSelector
	{
		private readonly IValidator<XElement> _itemValidator;

		public XmlItemSelector(IValidator<XElement> itemValidator)
		{
			_itemValidator = itemValidator;
		}

		public IEnumerable<XElement> SelectItems(Stream stream)
		{
			XDocument doc = XDocument.Load(stream);
			XNamespace ns = doc.Root.Name.Namespace;

			XElement items = doc.Root.Element(ns + "Items");
			if (items == null)
				throw new ArgumentException();
			if (!_itemValidator.IsValid(items))
				throw new ArgumentException();

			return items.Elements(ns + "Item");
		}
	}
}