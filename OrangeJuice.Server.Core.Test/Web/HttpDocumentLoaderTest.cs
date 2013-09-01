using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Test.Web
{
	[TestClass]
	public class HttpDocumentLoaderTest
	{
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_Url_Is_Null()
		{
			// Arange
			const string url = null;

			IDocumentLoader documentLoader = new HttpDocumentLoader();

			// Act
			Action action = () => documentLoader.Load(url);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("url");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_Url_Is_Empty()
		{
			// Arange
			const string url = "";

			IDocumentLoader documentLoader = new HttpDocumentLoader();

			// Act
			Action action = () => documentLoader.Load(url);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("url");
		}
	}
}