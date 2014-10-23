using System;
using System.Collections.Specialized;

using FluentAssertions;

using Moq;

using OrangeJuice.Server.Web;

using Xunit.Extensions;

namespace OrangeJuice.Server.Test.Web
{
	public class QueryStringNameValueCollectionTest
	{
		#region Tests
		[Theory]
		[InlineData(typeof(QueryStringNameValueCollection))]
		[InlineData(typeof(FSharp.Web.QueryStringNameValueCollection))]
		public void Get_Should_Return_Key_When_Value_Is_Not_Empty(Type type)
		{
			// Arrange
			const string key = "key";
			const string expected = "value";

			var urlEncoderMock = new Mock<IUrlEncoder>();

			var coll = CreateCollection(type, new NameValueCollection { { key, expected } }, urlEncoderMock.Object);

			// Act
			string actual = coll.Get(key);

			// Arrange
			actual.Should().Be(expected);
		}

		[Theory]
		[InlineData(typeof(QueryStringNameValueCollection))]
		[InlineData(typeof(FSharp.Web.QueryStringNameValueCollection))]
		public void Get_Should_Return_Empty_String_When_Value_Is_Null(Type type)
		{
			// Arrange
			const string key = "key";
			const string value = null;

			var coll = CreateCollection(type, new NameValueCollection { { key, value } });

			// Act
			string actual = coll.Get(key);

			// Arrange
			actual.Should().BeEmpty();
		}

		[Theory]
		[InlineData(typeof(QueryStringNameValueCollection))]
		[InlineData(typeof(FSharp.Web.QueryStringNameValueCollection))]
		public void ToString_Should_Format_Each_Key_Value_Pair(Type type)
		{
			// Arrange
			var coll = CreateCollection(type, new NameValueCollection { { "key", "value" } });

			// Act
			string result = coll.ToString();

			// Arrange
			result.Should().Be("key=value");
		}

		[Theory]
		[InlineData(typeof(QueryStringNameValueCollection))]
		[InlineData(typeof(FSharp.Web.QueryStringNameValueCollection))]
		public void ToString_Should_Call_UrlEncoder_Encode_Each_Key_Value_Pair(Type type)
		{
			// Arrange
			const string key = "key";
			const string value = "value";

			var urlEncoderMock = new Mock<IUrlEncoder>();
			urlEncoderMock.Setup(e => e.Encode(value));

			var coll = CreateCollection(type, new NameValueCollection { { key, value } }, urlEncoderMock.Object);

			// Act
			string str = coll.ToString();

			// Arrange
			urlEncoderMock.VerifyAll();
		}

		[Theory]
		[InlineData(typeof(QueryStringNameValueCollection))]
		[InlineData(typeof(FSharp.Web.QueryStringNameValueCollection))]
		public void ToString_Should_Call_Join_By_Ampersand_All_Key_Value_Pair(Type type)
		{
			// Arrange
			var coll = CreateCollection(type,
				new NameValueCollection
				{
					{ "key1", "value1" },
					{ "key2", "value2" }
				});

			// Act
			string result = coll.ToString();

			// Arrange
			result.Should().Be("key1=value1&key2=value2");
		}
		#endregion

		#region Helper methods
		private static NameValueCollection CreateCollection(Type type, NameValueCollection coll, IUrlEncoder urlEncoder = null)
		{
			return (NameValueCollection)Activator.CreateInstance(type,
				coll,
				urlEncoder ?? CreateEndoder());
		}

		private static IUrlEncoder CreateEndoder()
		{
			var urlEncoderMock = new Mock<IUrlEncoder>();
			urlEncoderMock.Setup(e => e.Encode(It.IsAny<string>())).Returns<string>(s => s);
			return urlEncoderMock.Object;
		}

		#endregion
	}
}