using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data.ResponseGroup
{
	public sealed class DefaultPipelineFilter : IPipelineFilter<ProductDescriptor, XElement, AwsProductSearchCriteria>
	{
		public ProductDescriptor Execute(ProductDescriptor d, XElement element, AwsProductSearchCriteria searchCriteria)
		{
			var nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			d.SourceProductId = (string)element.XPathSelectElement("x:ASIN", nm);
			d.DetailsPageUrl = new Uri((string)element.XPathSelectElement("x:DetailPageURL", nm));

			d.Title = (string)element.XPathSelectElement("x:ItemAttributes/x:Title", nm);
			d.ProductGroup = (string)element.XPathSelectElement("x:ItemAttributes/x:ProductGroup", nm);

			return d;
		}
	}
}