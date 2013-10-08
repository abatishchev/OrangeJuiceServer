using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlItemValidator : IValidator<XElement>
	{
		public bool IsValid(XElement item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", item.Name.Namespace.ToString());

			return (bool)item.XPathSelectElement("x:Request/x:IsValid", nm);
		}
	}
}