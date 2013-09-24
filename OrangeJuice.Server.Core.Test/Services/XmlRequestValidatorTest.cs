using System;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class XmlRequestValidatorTest
	{
		[TestMethod]
		public void IsValid_Should_Throw_Exception()
		{
			// Arange
			const XElement item = null;

			IValidator<XElement> validator = new XmlRequestValidator();

			// Act
			Action action = () => validator.IsValid(item);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("item");
		}
	}
}