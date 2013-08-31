using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Handlers;

namespace OrangeJuice.Server.Api.Test.Handlers
{
	[TestClass]
	public class AppKeyHandlerBaseTest
	{
		#region Test methods
		[TestMethod]
		public void SendAsync_Should_Return_Status_ErrorCode_When_IsValid_Returns_False()
		{
			// Arrange
			const HttpStatusCode expected = HttpStatusCode.InternalServerError;
			HandlerStub handler = new HandlerStub(false, expected);
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
			private readonly HttpStatusCode _errorCode;

			public HandlerStub(bool isValid, HttpStatusCode errorCode)
			{
				_isValid = isValid;
				_errorCode = errorCode;
			}

			internal override HttpStatusCode ErrorCode
			{
				get { return _errorCode; }
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