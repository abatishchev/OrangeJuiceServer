﻿using System;
using System.Linq;
using System.Xml.Linq;

using FluentAssertions;

using Xunit;

using Moq;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	public class XmlItemSelectorTest
	{
		#region Test Methods
		[Fact]
		public void SelectItems_Should_Throw_Exception_When_ItemValidator_Returns_False()
		{
			// Arrange
			XNamespace ns = "test";
			XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "false"),
				new XElement(ns + "Root",
					new XElement(ns + "Error", "error")));

			IValidator<XElement> validator = CreateValidator(false);
			IItemSelector selector = CreateSelector(validator);

			// Act
			Action action = () => selector.SelectItems(doc.ToString());

			// Assert
			action.ShouldThrow<ArgumentException>();
		}

		[Fact]
		public void SelectItems_Should_Return_Items_From_String()
		{
			// Arrange
			XNamespace ns = "test";
			XElement expected = new XElement(ns + "Item");
			XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "false"),
				new XElement(ns + "Root",
					new XElement(ns + "Items", expected)));

			IItemSelector selector = CreateSelector();

			// Act
			XElement actual = selector.SelectItems(doc.ToString()).First();

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
		#endregion
	}
}