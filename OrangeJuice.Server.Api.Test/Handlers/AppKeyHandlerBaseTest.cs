using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using OrangeJuice.Server.Api.Handlers;

namespace OrangeJuice.Server.Api.Test.Handlers
{
	[TestClass]
	public class AppKeyHandlerBaseTest
	{
		#region Test methods
		[TestMethod]
		public void SendAsync_Should_Return_Status_Forbidden_When_IsValid_Returns_False()
		{
			// Arrange
			const HttpStatusCode expected = HttpStatusCode.Forbidden;
			HandlerStub handler = new HandlerStub(false);
			HttpRequestMessage request = new HttpRequestMessage();

			// Act
			Task<HttpResponseMessage> task = handler.SendAsync(request);

			// Assert
			HttpStatusCode actual = task.Result.StatusCode;
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper classes
		private class HandlerStub : AppKeyHandlerBase
		{
			private readonly bool _isValid;

			public HandlerStub(bool isValid)
			{
				_isValid = isValid;
			}

			internal override bool IsValid(HttpRequestMessage request)
			{
				return _isValid;
			}

			public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
			{
				return SendAsync(request, CancellationToken.None);
			}
		}
		#endregion
	}
}