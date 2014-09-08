using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class XmlItemSelectorTest
	{
		#region Test Methods
		[TestMethod]
		public void SelectItems_Should_Throw_Exception_When_Doc_Has_No_Root()
		{
			// Arrange
			XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "false"));
			Stream stream = CreateStream(doc);

			IItemSelector selector = CreateSelector();

			// Act
			Action action = () => selector.SelectItems(stream);

			// Assert
			action.ShouldThrow<ArgumentException>();
		}

		[TestMethod]
		public void SelectItems_Should_Throw_Exception_When_Doc_Has_No_Items()
		{
			// Arrange
			XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "false"),
				new XElement("Root"));
			Stream stream = CreateStream(doc);

			IItemSelector selector = CreateSelector();

			// Act
			Action action = () => selector.SelectItems(stream);

			// Assert
			action.ShouldThrow<ArgumentException>();
		}

		[TestMethod]
		public void SelectItems_Should_Throw_Exception_When_ItemValidator_Returns_False()
		{
			// Arrange
			XNamespace ns = "test";
			XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "false"),
				new XElement(ns + "Root"));
			Stream stream = CreateStream(doc);

			IValidator<XElement> validator = CreateValidator(false);
			IItemSelector selector = CreateSelector(validator);

			// Act
			Action action = () => selector.SelectItems(stream);

			// Assert
			action.ShouldThrow<ArgumentException>();
		}

		[TestMethod]
		public void SelectItems_Should_Return_Items_From_Document()
		{
			// Arrange
			XNamespace ns = "test";
			object[] expected = { new XElement(ns + "Item") };
			XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "false"),
				new XElement(ns + "Root",
					new XElement(ns + "Items", expected)));
			Stream stream = CreateStream(doc);

			IItemSelector selector = CreateSelector();

			// Act
			XElement[] actual = selector.SelectItems(stream).ToArray();

			// Assert
			actual.Should().BeEquivalentTo(expected);
		}
		#endregion

		#region Helper methods
		private static IItemSelector CreateSelector(IValidator<XElement> validator = null)
		{
			return new XmlItemSelector(validator ?? CreateValidator());
		}

		private static IValidator<XElement> CreateValidator(bool isValid = true)
		{
			var validator = new Mock<IValidator<XElement>>();
			validator.Setup(v => v.IsValid(It.IsAny<XElement>())).Returns(isValid);
			return validator.Object;
		}

		private static Stream CreateStream(XDocument doc)
		{
			Stream stream = new MemoryStream();
			doc.Save(stream);
			stream.Seek(0, SeekOrigin.Begin);
			return stream;
		}
		#endregion
	}
}