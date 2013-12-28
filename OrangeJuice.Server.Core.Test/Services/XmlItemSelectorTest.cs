using System;
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
		public void SelectItemsShould_Throw_Exception_When_Doc_Has_No_Items()
		{
			// Arrange
			XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "false"),
				new XElement("Root"));

			IItemSelector selector = CreateSelector();

			// Act
			Action action = () => selector.SelectItems(doc);

			// Assert
			action.ShouldThrow<InvalidOperationException>();
		}

		[TestMethod]
		public void SelectItemsShould_Throw_Exception_When_ItemValidator_Returns_False()
		{
			// Arrange
			XNamespace ns = "test";
			XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "false"),
				new XElement(ns + "Root"));

			IValidator<XElement> validator = CreateValidator(false);
			IItemSelector selector = CreateSelector(validator);

			// Act
			Action action = () => selector.SelectItems(doc);

			// Assert
			action.ShouldThrow<InvalidOperationException>();
		}

		[TestMethod]
		public void SelectItemsShould_Return_Items_From_Document()
		{
			// Arrange
			XNamespace ns = "test";
			XElement[] expected = new[] { new XElement(ns + "Item") };
			XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "false"),
				new XElement(ns + "Root",
					new XElement(ns + "Items",
						expected)));

			IItemSelector selector = CreateSelector();

			// Act
			XElement[] actual = selector.SelectItems(doc).ToArray();

			// Assert
			actual.ShouldBeEquivalentTo(expected);
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
		#endregion
	}
}