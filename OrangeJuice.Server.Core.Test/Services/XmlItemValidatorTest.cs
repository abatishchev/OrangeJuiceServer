using System;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class XmlItemValidatorTest
	{
		[TestMethod]
		public void IsValid_Should_Throw_Exception()
		{
			// Arange
			const XElement item = null;

			IValidator<XElement> validator = new XmlItemValidator();

			// Act
			Action action = () => validator.IsValid(item);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("item");
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

			IValidator<XElement> validator = new XmlItemValidator();

			// Act
			bool actual = validator.IsValid(item);

			// Assert
			actual.Should().Be(expected);
		}
	}
}