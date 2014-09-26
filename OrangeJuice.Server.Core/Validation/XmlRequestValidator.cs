using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace OrangeJuice.Server.Validation
{
	public sealed class XmlRequestValidator : IValidator<XElement>
	{
		public bool IsValid(XElement item)
		{
			if (item == null)
				return false;

			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", item.Name.Namespace.ToString());

			return (bool)item.XPathSelectElement("x:Request/x:IsValid", nm);
		}
	}
}