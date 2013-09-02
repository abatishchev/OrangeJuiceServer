using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Test.Builders
{
	[TestClass]
	public class FlattenArgumentFormatterTest
	{
		#region Test methods
		[TestMethod]
		public void Ctor_Should_Throw_Exception_When_UrlEncoder_Is_Null()
		{
			// Arange
			const IUrlEncoder urlEncoder = null;

			// Act
			Action action = () => new FlattenArgumentFormatter(urlEncoder);

			// Assert
			action.ShouldThrow<ArgumentNullException>()
				  .And.ParamName.Should().Be("urlEncoder");
		}

		[TestMethod]
		public void BuildQuery_Should_Join_Dictionary_Pairs_By_Ampersand()
		{
			// Arrange
			var queryBuilder = CreateQueryBuilder();
			var dic = new Dictionary<string, string>
			{
				{ "a", "" },
				{ "b", "" }
			};

			// Act
			string query = queryBuilder.FormatArgs(dic);

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
				{ "a", "1" }
			};

			// Act
			string query = queryBuilder.FormatArgs(dic);

			// Assert
			query.Should().Contain("a=1");
		}
		#endregion

		#region Helper methods
		private static FlattenArgumentFormatter CreateQueryBuilder(IUrlEncoder urlEncoder = null)
		{
			return new FlattenArgumentFormatter(urlEncoder ?? CreateUrlEncoder().Object);
		}

		private static Mock<IUrlEncoder> CreateUrlEncoder()
		{
			var urlEncoderMock = new Mock<IUrlEncoder>();
			urlEncoderMock.Setup(e => e.Encode(It.IsAny<string>())).Returns<string>(s => s);
			return urlEncoderMock;
		}
		#endregion
	}
}