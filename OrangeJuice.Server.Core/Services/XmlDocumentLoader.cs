using System.IO;
using System.Xml.Linq;

namespace OrangeJuice.Server.Web
{
	public sealed class XmlDocumentLoader : IDocumentLoader
	{
		public XDocument Load(Stream stream)
		{
			return XDocument.Load(stream);
		}
	}
}