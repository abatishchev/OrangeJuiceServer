using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data.ResponseGroup
{
	public sealed class OfferSummaryPipelineFilter : IPipelineFilter<ProductDescriptor, XElement, AwsProductSearchCriteria>
	{
		public ProductDescriptor Execute(ProductDescriptor d, XElement element, AwsProductSearchCriteria searchCriteria)
		{
			if (searchCriteria.ResponseGroups.Contains("OfferSummary"))
			{
				var nm = new XmlNamespaceManager(new NameTable());
				nm.AddNamespace("x", element.Name.Namespace.ToString());

				d.LowestNewPrice = (float?)element.XPathSelectElement("x:OfferSummary/x:LowestNewPrice/x:Amount", nm);
			}
			return d;
		}
	}
}