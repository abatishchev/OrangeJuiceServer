using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Web;

using IStringDictionary = System.Collections.Generic.IDictionary<string, string>;
using StringDictionary = System.Collections.Generic.Dictionary<string, string>;

namespace OrangeJuice.Server.Test.Web
{
	[TestClass]
	public class EncodedQueryBuilderTest
	{
		[TestMethod]
		public void BuildUrl_Should_Call_UrlEncoder_Encode_For_Each_Argument_Value()
		{
			// Arrange
			var encoderMock = new Mock<IUrlEncoder>();
			encoderMock.Setup(e => e.Encode(It.IsAny<string>())).Returns<string>(s => s);

			IQueryBuilder urlBuilder = new EncodedQueryBuilder(encoderMock.Object);

			IStringDictionary args = new StringDictionary { { "key", "value" } };

			// Act
			urlBuilder.BuildQuery(args);

			// Assert
			encoderMock.Verify(e => e.Encode(It.IsAny<string>()), Times.Exactly(args.Count));
		}
	}
}