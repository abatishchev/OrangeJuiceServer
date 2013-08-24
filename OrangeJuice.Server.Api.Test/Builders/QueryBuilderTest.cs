using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Builders;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Api.Test.Builders
{
	[TestClass]
	public class QueryBuilderTest
	{
		#region Test methods
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_AccessKey_Is_Null()
		{
			// Arange
			const string accessKey = null;
			const IUrlEncoder urlEncoder = null;
			const IDateTimeProvider dateTimeProvider = null;

			// Act
			Action action = () => new QueryBuilder(accessKey, urlEncoder, dateTimeProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("accessKey");
		}

		public void Ctor_Should_Throw_Exception_When_AccessKey_Is_Empty()
		{
			// Arange
			const string accessKey = "";
			const IUrlEncoder urlEncoder = null;
			const IDateTimeProvider dateTimeProvider = null;

			// Act
			Action action = () => new QueryBuilder(accessKey, urlEncoder, dateTimeProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("accessKey");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_UrlEncoder_Is_Null()
		{
			// Arange
			const string accessKey = "accessKey";
			const IUrlEncoder urlEncoder = null;
			const IDateTimeProvider dateTimeProvider = null;

			// Act
			Action action = () => new QueryBuilder(accessKey, urlEncoder, dateTimeProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("urlEncoder");
		}

		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_DateTimeProvider_Is_Null()
		{
			// Arange
			const string accessKey = "accessKey";
			IUrlEncoder urlEncoder = new Mock<IUrlEncoder>().Object;
			const IDateTimeProvider dateTimeProvider = null;

			// Act
			Action action = () => new QueryBuilder(accessKey, urlEncoder, dateTimeProvider);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("dateTimeProvider");
		}

		[TestMethod]
		public void BuildQuery_Should_Return_Query_Containing_AwsAccessKeyId()
		{
			// Arange
			const string accessKey = "accessKey";
			var urlEncoderMock = CreateUrlEncoder();
			var queryBuilder = CreateQueryBuilder(urlEncoder: urlEncoderMock.Object);
			var dic = new Dictionary<string, string>();

			// Act
			string query = queryBuilder.BuildQuery(dic);

			// Assert
			var queryParams = BuildQeeryParamsDictionary(query);
			queryParams.Should().Contain(QueryBuilder.AwsAccessKeyId, accessKey);
		}

		[TestMethod]
		public void BuildQuery_Should_Return_Query_Containing_Timestamp()
		{
			// Arange
			const string timeStamp = "2012-08-01";
			var dateTimeProviderMock = new Mock<IDateTimeProvider>();
			dateTimeProviderMock.Setup(p => p.FormatToUniversal(It.IsAny<DateTime>())).Returns(timeStamp);

			var queryBuilder = CreateQueryBuilder(dateTimeProvider: dateTimeProviderMock.Object);
			var dic = new Dictionary<string, string>();

			// Act
			string query = queryBuilder.BuildQuery(dic);

			// Assert
			var queryParams = BuildQeeryParamsDictionary(query);
			queryParams.Should().Contain(QueryBuilder.Timestamp, timeStamp);
		}

		[TestMethod]
		public void BuildQuery_Should_Call_DateTimeProvider_GetNow()
		{
			// Arrange
			var dateTimeProviderMock = CreateDateTimeProvider(DateTime.UtcNow);

			var queryBuilder = CreateQueryBuilder(dateTimeProvider: dateTimeProviderMock.Object);
			var dic = new Dictionary<string, string>();

			// Act
			queryBuilder.BuildQuery(dic);

			// Assert
			dateTimeProviderMock.Verify(p => p.GetNow(), Times.Once());
		}

		[TestMethod]
		public void BuildQuery_Should_Call_DateTimeProvider_FormatToUniversal()
		{
			// Arrange
			var dateTimeProviderMock = CreateDateTimeProvider(DateTime.UtcNow);

			var queryBuilder = CreateQueryBuilder(dateTimeProvider: dateTimeProviderMock.Object);
			var dic = new Dictionary<string, string>();

			// Act
			queryBuilder.BuildQuery(dic);

			// Assert
			dateTimeProviderMock.Verify(p => p.FormatToUniversal(It.IsAny<DateTime>()), Times.Once());
		}

		[TestMethod]
		public void BuildQuery_Should_Pass_To_DateTimeProvider_FormatToUniversal_DateTime_UtcNow()
		{
			// Arrange
			DateTime now = DateTime.UtcNow;
			var dateTimeProviderMock = CreateDateTimeProvider(now);

			var queryBuilder = CreateQueryBuilder(dateTimeProvider: dateTimeProviderMock.Object);
			var dic = new Dictionary<string, string>();

			// Act
			queryBuilder.BuildQuery(dic);

			// Assert
			dateTimeProviderMock.Verify(p => p.FormatToUniversal(now));
		}

		[TestMethod]
		public void BuildQuery_Should_Join_Dictionary_Pairs_By_Ampersand()
		{
			// Arrange
			var queryBuilder = CreateQueryBuilder();
			var dic = new Dictionary<string, string>
			{
				{"a", ""},
				{"b", ""}
			};

			// Act
			string query = queryBuilder.BuildQuery(dic);

			// Assert
			query.Should().Contain("a=&b=");
		}

		[TestMethod]
		public void BuildQuery_Should_Join_Dictionary_Key_And_Value_By_Equals_Sign()
		{
			// Arrange
			var queryBuilder = CreateQueryBuilder();
			var dic = new Dictionary<string, string>
			{
				{"a", "1"}
			};

			// Act
			string query = queryBuilder.BuildQuery(dic);

			// Assert
			query.Should().Contain("a=1");
		}
		#endregion

		#region Helper methods
		private static QueryBuilder CreateQueryBuilder(IUrlEncoder urlEncoder = null, IDateTimeProvider dateTimeProvider = null)
		{
			const string accessKey = "accessKey";
			IUrlEncoder urlEncoderMock = urlEncoder ?? CreateUrlEncoder().Object;
			IDateTimeProvider dateTimeProviderMock = dateTimeProvider ?? CreateDateTimeProvider(DateTime.UtcNow).Object;

			return new QueryBuilder(accessKey, urlEncoderMock, dateTimeProviderMock);
		}

		private static Mock<IUrlEncoder> CreateUrlEncoder()
		{
			var urlEncoderMock = new Mock<IUrlEncoder>();
			urlEncoderMock.Setup(e => e.Encode(It.IsAny<string>())).Returns<string>(s => s);
			return urlEncoderMock;
		}

		private static Mock<IDateTimeProvider> CreateDateTimeProvider(DateTime now)
		{
			var dateTimeProviderMock = new Mock<IDateTimeProvider>();
			dateTimeProviderMock.Setup(p => p.GetNow()).Returns(now);
			dateTimeProviderMock.Setup(p => p.FormatToUniversal(now)).Returns(now.ToString());
			return dateTimeProviderMock;
		}

		private static IDictionary<string, string> BuildQeeryParamsDictionary(string query)
		{
			return query.Split('&')
						.Select(s => new { s, p = s.Split('=') })
						.Select(t => new { Key = t.p[0], Value = t.p[1] })
						.ToDictionary(x => x.Key, x => x.Value);
		}
		#endregion
	}
}