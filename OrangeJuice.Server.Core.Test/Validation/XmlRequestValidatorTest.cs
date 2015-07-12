using System;
using System.Xml.Linq;

using FluentAssertions;

using OrangeJuice.Server.Validation;

using Xunit;

namespace OrangeJuice.Server.Test.Validation
{
	public class XmlRequestValidatorTest
	{
		#region Tests
		[Theory]
		[InlineData(typeof(XmlRequestValidator))]
		[InlineData(typeof(FSharp.Validation.XmlRequestValidator))]
		public void IsValid_Should_Return_False_When_Item_Is_Null(Type type)
		{
			// Arange
			const XElement item = null;

			IValidator<XElement> validator = CreateValidator(type);

			// Act
			bool actual = validator.IsValid(item);

			// Assert
			actual.Should().BeFalse();
		}

		[Theory]
		[InlineData(typeof(XmlRequestValidator))]
		[InlineData(typeof(FSharp.Validation.XmlRequestValidator))]
		public void IsValid_Should_Return_Request_Element_IsValid_Value(Type type)
		{
			// Arange
			const bool expected = true;
			XNamespace ns = "test";
			XElement item = new XElement(ns + "Item",
				new XElement(ns + "Request",
					new XElement(ns + "IsValid",
						new XText(Convert.ToString(expected)))));

			IValidator<XElement> validator = CreateValidator(type);

			// Act
			bool actual = validator.IsValid(item);

			// Assert
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper methods

		private static IValidator<XElement> CreateValidator(Type type)
		{
			return (IValidator<XElement>)Activator.CreateInstance(type);
		}
		#endregion
	}
}