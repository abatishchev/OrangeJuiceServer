using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace OrangeJuice.Server.Data
{
	public sealed class XmlProductDescriptorFactory : IProductDescriptorFactory<XElement>
	{
		#region IProductDescriptorFactory members
		public ProductDescriptor Create(XElement element)
		{
			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			return new ProductDescriptor
			{
				SourceProductId = (string)element.XPathSelectElement("x:ASIN", nm),

				Title = (string)element.XPathSelectElement("x:ItemAttributes/x:Title", nm),
				Brand = (string)element.XPathSelectElement("x:ItemAttributes/x:Brand", nm),

				SmallImageUrl = (string)element.XPathSelectElement("x:SmallImage/x:URL", nm),
				MediumImageUrl = (string)element.XPathSelectElement("x:MediumImage/x:URL", nm),
				LargeImageUrl = (string)element.XPathSelectElement("x:LargeImage/x:URL", nm),
			};
		}
		#endregion
	}
}