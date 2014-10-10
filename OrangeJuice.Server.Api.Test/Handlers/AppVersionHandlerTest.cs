using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;
using Xunit;
using Moq;

using OrangeJuice.Server.Api.Handlers;

namespace OrangeJuice.Server.Api.Test.Handlers
{
	public class AppVersionHandlerTest
	{
		#region Test methods
		[Fact]
		public void SendAsync_Should_Return_Status_Forbidden_When_IsValid_Returns_False()
		{
			// Arrange
			const bool isValid = false;
			const HttpStatusCode expected = HttpStatusCode.Forbidden;

			var validatorMock = new Mock<IValidator<HttpRequestMessage>>();
			validatorMock.Setup(v => v.IsValid(It.IsAny<HttpRequestMessage>())).Returns(isValid);

			HandlerStub handler = new HandlerStub(validatorMock.Object);
			HttpRequestMessage request = new HttpRequestMessage();

			// Act
			Task<HttpResponseMessage> task = handler.SendAsync(request);

			// Assert
			HttpStatusCode actual = task.Result.StatusCode;
			actual.Should().Be(expected);
		}
		#endregion

		#region Helper classes
		private class HandlerStub : AppVersionHandler
		{
			public HandlerStub(IValidator<HttpRequestMessage> requestValidator)
				: base(requestValidator)
			{
			}

			public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
			{
				return SendAsync(request, CancellationToken.None);
			}
		}
		#endregion
	}
}