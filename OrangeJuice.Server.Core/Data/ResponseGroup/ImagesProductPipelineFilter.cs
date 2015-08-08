using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data.ResponseGroup
{
	public sealed class ImagesProductPipelineFilter : IPipelineFilter<ProductDescriptor, XElement, AwsProductSearchCriteria>
	{
		public ProductDescriptor Execute(ProductDescriptor d, XElement element, AwsProductSearchCriteria searchCriteria)
		{
			if (searchCriteria.ResponseGroups.Contains("Images"))
			{
				var nm = new XmlNamespaceManager(new NameTable());
				nm.AddNamespace("x", element.Name.Namespace.ToString());

				d.SmallImageUrl = (string)element.XPathSelectElement("x:SmallImage/x:URL", nm);
				d.MediumImageUrl = (string)element.XPathSelectElement("x:MediumImage/x:URL", nm);
				d.LargeImageUrl = (string)element.XPathSelectElement("x:LargeImage/x:URL", nm);
			}
			return d;
		}
	}
}