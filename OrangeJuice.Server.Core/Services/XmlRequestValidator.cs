using System;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlRequestValidator : IRequestValidator
	{
		// ReSharper disable once PossibleNullReferenceException
		public bool IsValid(XElement items)
		{
			if (items == null)
				throw new ArgumentNullException("items");

			XNamespace ns = items.Name.Namespace;
			return (bool)items.Element(ns + "Request")
							  .Element(ns + "IsValid");
		}
	}
}