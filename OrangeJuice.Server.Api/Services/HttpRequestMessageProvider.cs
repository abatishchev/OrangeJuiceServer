using System;
using System.Net.Http;

using SimpleInjector;

namespace OrangeJuice.Server.Api.Services
{
	public sealed class HttpRequestMessageProvider : Web.IRequestMessageProvider
	{
		private readonly Lazy<HttpRequestMessage> _message;

		public HttpRequestMessageProvider(Container container)
		{
			_message = new Lazy<HttpRequestMessage>(container.GetCurrentHttpRequestMessage);
		}

		public HttpRequestMessage CurrentMessage
		{
			get { return _message.Value; }
		}
	}
}