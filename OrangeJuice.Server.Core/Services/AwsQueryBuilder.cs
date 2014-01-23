using System;
using System.Collections.Generic;
using System.Linq;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public class AwsQueryBuilder : IQueryBuilder
	{
		#region Constants
		private const string RequestHost = "webservices.amazon.com";
		private const string RequestPath = "/onca/xml";
		#endregion

		#region Fields
		private readonly IArgumentBuilder _argumentBuilder;
		private readonly IQuerySigner _querySigner;
		private readonly IUrlEncoder _urlEncoder;
		#endregion

		#region Ctor
		public AwsQueryBuilder(IArgumentBuilder argumentBuilder, IQuerySigner querySigner, IUrlEncoder urlEncoder)
		{
			_argumentBuilder = argumentBuilder;
			_querySigner = querySigner;
			_urlEncoder = urlEncoder;
		}
		#endregion

		#region IQueryBuilder members
		public Uri BuildUrl(IDictionary<string, string> args)
		{
			args = _argumentBuilder.BuildArgs(args);

			string query = BuildQuery(args);

			string signature = _querySigner.SignQuery(RequestHost, RequestPath, query);
			args.Add("Signature", signature);

			query = BuildQuery(args);

			return new UriBuilder(Uri.UriSchemeHttp, RequestHost, 80, RequestPath, '?' + query).Uri;
		}

		private string BuildQuery(IEnumerable<KeyValuePair<string, string>> args)
		{
			return String.Join("=",
				args.Select(p => String.Format("{0}={1}",
					p.Key,
					_urlEncoder.Encode(p.Value))));
		}
		#endregion
	}
}