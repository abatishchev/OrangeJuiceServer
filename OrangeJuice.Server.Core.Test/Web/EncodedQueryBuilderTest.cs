using System;

using FluentAssertions;

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
		#region Test methods
		[TestMethod]
		public void BuildQuery_Should_Call_UrlEncoder_Encode_For_Each_Argument_Value()
		{
			// Arrange
			var encoderMock = CreateEncoder();

			IQueryBuilder urlBuilder = CreatelBuilder(encoderMock.Object);

			IStringDictionary args = new StringDictionary { { "key", "value" } };

			// Act
			urlBuilder.BuildQuery(args);

			// Assert
			encoderMock.Verify(e => e.Encode(It.IsAny<string>()), Times.Exactly(args.Count));
		}

		[TestMethod]
		public void BuildQuery_Should_Split_Name_And_Value()
		{
			// Arrange
			IQueryBuilder urlBuilder = CreatelBuilder();

			IStringDictionary args = new StringDictionary { { "key", "value" } };

			// Act
			string query = urlBuilder.BuildQuery(args);

			// Assert
			query.Should().Be("key=value");
		}

		[TestMethod]
		public void BuildQuery_Should_Split_Parameters()
		{
			// Arrange
			IQueryBuilder urlBuilder = CreatelBuilder();

			IStringDictionary args = new StringDictionary { { "a", "1" }, { "b", "2" } };

			// Act
			string query = urlBuilder.BuildQuery(args);

			// Assert
			query.Should().Be("a=1&b=2");
		}

		[TestMethod]
		public void SignQuery_Should_Call_UrlEncoder_Encode_For_Signature()
		{
			// Arrange
			const string query = "query";
			const string signature = "signature";

			var encoderMock = CreateEncoder();

			IQueryBuilder urlBuilder = CreatelBuilder(encoderMock.Object);

			// Act
			urlBuilder.SignQuery(query, signature);

			// Assert
			encoderMock.Verify(e => e.Encode(signature), Times.Once);
		}

		[TestMethod]
		public void SignQuery_Should_Append_Signature()
		{
			// Arrange
			const string query = "query";
			const string signature = "signature";

			var encoderMock = CreateEncoder();

			IQueryBuilder urlBuilder = CreatelBuilder(encoderMock.Object);

			// Act
			string signedQuery = urlBuilder.SignQuery(query, signature);

			// Assert
			signedQuery.Should().EndWith(String.Format("&Signature={0}", signature));
		}
		#endregion

		#region Helper methods
		private static EncodedQueryBuilder CreatelBuilder(IUrlEncoder urlEncoder = null)
		{
			return new EncodedQueryBuilder(urlEncoder ?? CreateEncoder().Object);
		}

		private static Mock<IUrlEncoder> CreateEncoder()
		{
			var encoderMock = new Mock<IUrlEncoder>();
			encoderMock.Setup(e => e.Encode(It.IsAny<string>())).Returns<string>(s => s);
			return encoderMock;
		}
		#endregion
	}
}