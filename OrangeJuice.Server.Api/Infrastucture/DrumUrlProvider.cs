using System;
using System.Linq.Expressions;
using System.Net.Http;

using Ab.Factory;
using Ab.Web;

using Drum;

namespace OrangeJuice.Server.Api.Infrastucture
{
	public sealed class DrumUrlProvider : IUrlProvider
	{
		private readonly UriMakerContext _context;
		private readonly IFactory<HttpRequestMessage> _requestMessageFactory;

		public DrumUrlProvider(UriMakerContext context, IFactory<HttpRequestMessage> requestMessageFactory)
		{
			_context = context;
			_requestMessageFactory = requestMessageFactory;
		}

		public Uri UriFor<TController>(Expression<Action<TController>> action)
		{
			var maker = new UriMaker<TController>(_context, _requestMessageFactory.Create());
			return maker.UriFor(action);
		}
	}
}