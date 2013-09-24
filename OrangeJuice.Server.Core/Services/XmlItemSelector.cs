using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlItemSelector : IItemSelector
	{
		private readonly IValidator<XElement> _requestValidator;

		public XmlItemSelector(IValidator<XElement> requestValidator)
		{
			if (requestValidator == null)
				throw new ArgumentNullException("requestValidator");
			_requestValidator = requestValidator;
		}

		public IEnumerable<XElement> GetItems(XDocument doc)
		{
			if (doc == null)
				throw new ArgumentNullException("doc");
			if (doc.Root == null)
				throw new ArgumentNullException("doc");

			XNamespace ns = doc.Root.Name.Namespace;

			XElement items = doc.Root.Element(ns + "Items");
			if (items == null || !_requestValidator.IsValid(items))
				throw new InvalidOperationException("Response is not valid");

			return items.Elements(ns + "Item");
		}
	}
}