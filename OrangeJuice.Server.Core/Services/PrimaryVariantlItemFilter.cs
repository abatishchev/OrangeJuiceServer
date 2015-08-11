using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using OrangeJuice.Server.Filters;

namespace OrangeJuice.Server.Services
{
	public sealed class PrimaryVariantlItemFilter : IFilter<XElement>
	{
		public bool Filter(XElement element)
		{
			var nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			return !element.XPathSelectElements("x:ImageSets/x:ImageSet[contains(@Category, 'variant')]", nm).Any();
		}
	}
}