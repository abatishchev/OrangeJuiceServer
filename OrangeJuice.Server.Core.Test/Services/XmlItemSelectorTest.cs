using System;
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
		public void Ctor_Should_Throw_Exception_When_RequestValidator_Is_Null()
		{
			// Arrange
			const IValidator<XElement> validator = null;

			// Act
			Action action = () => new XmlItemSelector(validator);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("requestValidator");
		}

		[TestMethod]
		public void GetItems_Should_Throw_Exception_When_Doc_Is_Null()
		{
			// Arrange
			const XDocument doc = null;

			IItemSelector selector = CreateValidator();

			// Act
			Action action = () => selector.GetItems(doc);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("doc");
		}

		[TestMethod]
		public void GetItems_Should_Throw_Exception_When_Doc_Root_Is_Null()
		{
			// Arrange
			XDocument doc = new XDocument();

			IItemSelector selector = CreateValidator();

			// Act
			Action action = () => selector.GetItems(doc);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("doc");
		}

		[TestMethod]
		public void GetItems_Should_()
		{
			Assert.Inconclusive("TODO");
		}
		#endregion

		#region Helper methods
		private static IItemSelector CreateValidator(IValidator<XElement> validator = null)
		{
			return new XmlItemSelector(validator ?? new Mock<IValidator<XElement>>().Object);
		}
		#endregion
	}
}