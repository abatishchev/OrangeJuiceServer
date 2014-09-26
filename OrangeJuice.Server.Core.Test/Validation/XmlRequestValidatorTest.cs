using System;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Validation;

namespace OrangeJuice.Server.Test.Validation
{
	[TestClass]
	public class XmlRequestValidatorTest
	{
		[TestMethod]
		public void IsValid_Should_Return_False_When_Item_Is_Null()
		{
			// Arange
			const XElement item = null;

			IValidator<XElement> validator = new XmlRequestValidator();

			// Act
			bool actual = validator.IsValid(item);

			// Assert
			actual.Should().BeFalse();
		}

		[TestMethod]
		public void IsValid_Should_Return_Request_IsValid_Element_Value()
		{
			// Arange
			const bool expected = true;
			XNamespace ns = "test";
			XElement item = new XElement(ns + "Item",
				new XElement(ns + "Request",
					new XElement(ns + "IsValid",
						new XText(Convert.ToString(expected)))));

			IValidator<XElement> validator = new XmlRequestValidator();

			// Act
			bool actual = validator.IsValid(item);

			// Assert
			actual.Should().Be(expected);
		}
	}
}