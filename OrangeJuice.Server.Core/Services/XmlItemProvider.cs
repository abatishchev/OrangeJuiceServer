using System;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlItemProvider : IItemProvider
	{
		private readonly IRequestValidator _requestValidator;

		public XmlItemProvider(IRequestValidator requestValidator)
		{
			if (requestValidator == null)
				throw new ArgumentNullException("requestValidator");
			_requestValidator = requestValidator;
		}

		// ReSharper disable once PossibleNullReferenceException
		public XElement GetItems(XDocument doc)
		{
			if (doc == null)
				throw new ArgumentNullException("doc");

			XNamespace ns = doc.Root.Name.Namespace;

			var items = doc.Root.Element(ns + "Items");
			if (!_requestValidator.IsValid(items))
				throw new InvalidOperationException("Response is not valid");

			return items;
		}
	}
}