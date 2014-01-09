using System.Collections.Generic;
using System.Net.Http;
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
			if (doc.Root == null)
				throw new HttpRequestException();

			XNamespace ns = doc.Root.Name.Namespace;

			XElement items = doc.Root.Element(ns + "Items");
			if (items == null)
				throw new HttpRequestException();
			if (!_itemValidator.IsValid(items))
				throw new HttpRequestException();

			return items.Elements(ns + "Item");
		}
	}
}