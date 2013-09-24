using System;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlItemValidator : IValidator<XElement>
	{
		// ReSharper disable once PossibleNullReferenceException
		public bool IsValid(XElement item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			XNamespace ns = item.Name.Namespace;
			return (bool)item.Element(ns + "Request")
			                 .Element(ns + "IsValid");
		}
	}
}