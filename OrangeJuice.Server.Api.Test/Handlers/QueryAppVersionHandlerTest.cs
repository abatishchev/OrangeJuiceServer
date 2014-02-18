﻿using System;
using System.Net.Http;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Handlers;

namespace OrangeJuice.Server.Api.Test.Handlers
{
	[TestClass]
	public class QueryAppVersionHandlerTest
	{
		#region Test methods
		[TestMethod]
		public void ValidateKey_Should_Return_True_When_Query_Contains_AppVersion()
		{
			// Arrange
			Version appVersion = new Version();
			QueryAppVersionHandler handler = CreateHandler(appVersion);
			HttpRequestMessage request = CreateRequest("appVer", appVersion);

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateKey_Should_Return_False_When_Query_Does_Not_Contain_AppVersion()
		{
			// Arrange
			QueryAppVersionHandler handler = CreateHandler();
			HttpRequestMessage request = CreateRequest("any-name", "any-value");

			// Act
			bool valid = handler.IsValid(request);

			// Assert
			valid.Should().BeFalse();
		}
		#endregion

		#region Helper methods
		private static QueryAppVersionHandler CreateHandler(Version appVersion = null)
		{
			return new QueryAppVersionHandler(appVersion ?? new Version());
		}

		private static HttpRequestMessage CreateRequest(string name, object value)
		{
			return new HttpRequestMessage
			{
				RequestUri = new UriBuilder(Uri.UriSchemeHttp, "example.com", 80, "", String.Format("?{0}={1}", name, value)).Uri
			};
		}
		#endregion
	}
}