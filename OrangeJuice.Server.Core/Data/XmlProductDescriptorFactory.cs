using System.Xml;
using System.Xml.Linq;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
{
	public sealed class XmlProductDescriptorFactory : Factory.IFactory<ProductDescriptor, XElement, AwsProductSearchCriteria>
	{
		private readonly IPipeline<ProductDescriptor, XElement, AwsProductSearchCriteria> _pipeline;

		public XmlProductDescriptorFactory(IPipeline<ProductDescriptor, XElement, AwsProductSearchCriteria> pipeline)
		{
			_pipeline = pipeline;
		}

		public ProductDescriptor Create(XElement element, AwsProductSearchCriteria searchCriteria)
		{
			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			return _pipeline.Execute(new ProductDescriptor(), element, searchCriteria);
		}
	}
}