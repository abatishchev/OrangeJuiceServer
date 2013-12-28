using System;
using System.Collections.Generic;
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

		public IEnumerable<XElement> SelectItems(XDocument doc)
		{
			XNamespace ns = doc.Root.Name.Namespace;

			XElement items = doc.Root.Element(ns + "Items");
			if (items == null || !_itemValidator.IsValid(items))
				throw new InvalidOperationException("Response is not valid");

			return items.Elements(ns + "Item");
		}
	}
}