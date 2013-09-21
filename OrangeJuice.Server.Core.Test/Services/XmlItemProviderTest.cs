using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Test.Services
{
	[TestClass]
	public class XmlItemProviderTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_RequestValidator_Is_Null()
		{
			// Arrange
			const IRequestValidator requestValidator = null;

			// Act
			Action action = () => new XmlItemSelector(requestValidator);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("requestValidator");
		}
	}
}
