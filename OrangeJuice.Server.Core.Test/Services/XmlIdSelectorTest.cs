using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class XmlIdSelectorTest
	{
		[TestMethod]
		public void GetId_Should_Return_Asin_From_Element()
		{
			// Arrange
			const string expected = "ASIN";
			const string ns = "ns";

			XElement element = new XElement(XName.Get("anyElement", ns),
				new XElement(XName.Get("ASIN", ns),
					expected));

			IIdSelector selector = new XmlIdSelector();

			// Act
			string actual = selector.GetId(element);

			// Assert
			actual.Should().Be(expected);
		}
	}
}