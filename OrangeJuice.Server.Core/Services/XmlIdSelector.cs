using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace OrangeJuice.Server.Services
{
	public sealed class XmlIdSelector : IIdSelector
	{
		public string GetId(XElement element)
		{
			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			return (string)element.XPathSelectElement("x:ASIN", nm);
		}
	}
}