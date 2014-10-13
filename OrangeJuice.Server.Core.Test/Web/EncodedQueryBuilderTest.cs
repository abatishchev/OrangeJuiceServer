using System;

using FluentAssertions;

using Moq;

using OrangeJuice.Server.Web;

using Xunit.Extensions;

using IStringDictionary = System.Collections.Generic.IDictionary<string, string>;
using StringDictionary = System.Collections.Generic.Dictionary<string, string>;

namespace OrangeJuice.Server.Test.Web
{
	public class EncodedQueryBuilderTest
	{
		#region Tests
		[Theory]
		[InlineData(typeof(EncodedQueryBuilder))]
		[InlineData(typeof(FSharp.Web.EncodedQueryBuilder))]
		public void BuildQuery_Should_Pass_Each_Argument_Value_To_UrlEncoder_Encode(Type type)
		{
			// Arrange
			var encoderMock = CreateEncoder();

			IQueryBuilder urlBuilder = CreatelBuilder(type, encoderMock.Object);

			IStringDictionary args = new StringDictionary { { "key", "value" } };

			// Act
			urlBuilder.BuildQuery(args);

			// Assert
			encoderMock.Verify(e => e.Encode(It.IsAny<string>()), Times.Exactly(args.Count));
		}

		[Theory]
		[InlineData(typeof(EncodedQueryBuilder))]
		[InlineData(typeof(FSharp.Web.EncodedQueryBuilder))]
		public void BuildQuery_Should_Split_Name_And_Value(Type type)
		{
			// Arrange
			IQueryBuilder urlBuilder = CreatelBuilder(type);

			IStringDictionary args = new StringDictionary { { "key", "value" } };

			// Act
			string query = urlBuilder.BuildQuery(args);

			// Assert
			query.Should().Be("key=value");
		}

		[Theory]
		[InlineData(typeof(EncodedQueryBuilder))]
		[InlineData(typeof(FSharp.Web.EncodedQueryBuilder))]
		public void BuildQuery_Should_Split_Parameters(Type type)
		{
			// Arrange
			IQueryBuilder urlBuilder = CreatelBuilder(type);

			IStringDictionary args = new StringDictionary { { "a", "1" }, { "b", "2" } };

			// Act
			string query = urlBuilder.BuildQuery(args);

			// Assert
			query.Should().Be("a=1&b=2");
		}

		[Theory]
		[InlineData(typeof(EncodedQueryBuilder))]
		[InlineData(typeof(FSharp.Web.EncodedQueryBuilder))]
		public void SignQuery_Should_Pass_Signature_To_UrlEncoder_Encode(Type type)
		{
			// Arrange
			const string query = "query";
			const string signature = "signature";

			var encoderMock = CreateEncoder();

			IQueryBuilder urlBuilder = CreatelBuilder(type, encoderMock.Object);

			// Act
			urlBuilder.SignQuery(query, signature);

			// Assert
			encoderMock.Verify(e => e.Encode(signature), Times.Once);
		}

		[Theory]
		[InlineData(typeof(EncodedQueryBuilder))]
		[InlineData(typeof(FSharp.Web.EncodedQueryBuilder))]
		public void SignQuery_Should_Append_Signature(Type type)
		{
			// Arrange
			const string query = "query";
			const string signature = "signature";

			var encoderMock = CreateEncoder();

			IQueryBuilder urlBuilder = CreatelBuilder(type, encoderMock.Object);

			// Act
			string signedQuery = urlBuilder.SignQuery(query, signature);

			// Assert
			signedQuery.Should().EndWith(String.Format("&Signature={0}", signature));
		}
		#endregion

		#region Helper methods
		private static IQueryBuilder CreatelBuilder(Type type, IUrlEncoder urlEncoder = null)
		{
			return (IQueryBuilder)Activator.CreateInstance(type,
				urlEncoder ?? CreateEncoder().Object);
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