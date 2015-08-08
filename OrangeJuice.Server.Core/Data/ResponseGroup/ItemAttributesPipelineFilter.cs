using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data.ResponseGroup
{
	public sealed class ItemAttributesPipelineFilter : IPipelineFilter<ProductDescriptor, XElement, AwsProductSearchCriteria>
	{
		public ProductDescriptor Execute(ProductDescriptor d, XElement element, AwsProductSearchCriteria searchCriteria)
		{
			if (searchCriteria.ResponseGroups.Contains("ItemAttributes"))
			{
				var nm = new XmlNamespaceManager(new NameTable());
				nm.AddNamespace("x", element.Name.Namespace.ToString());

				d.Brand = (string)element.XPathSelectElement("x:ItemAttributes/x:Brand", nm);
				d.CustomerReviewsUrl = new Uri((string)element.XPathSelectElement("x:ItemLinks/x:ItemLink[x:Description/. = 'All Customer Reviews']/x:URL", nm));
			}
			return d;
		}
	}
}