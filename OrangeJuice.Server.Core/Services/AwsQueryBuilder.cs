using System;
using System.Collections.Generic;
using System.Collections.Specialized;

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
		#endregion

		#region Ctor
		public AwsQueryBuilder(IArgumentBuilder argumentBuilder, IQuerySigner querySigner)
		{
			_argumentBuilder = argumentBuilder;
			_querySigner = querySigner;
		}
		#endregion

		#region IquerBuilder members
		public string BuildUrl(IDictionary<string, string> args)
		{
			NameValueCollection collection = _argumentBuilder.BuildArgs(args);

			string signature = _querySigner.SignQuery(RequestHost, RequestPath, collection.ToString());
			collection.Add("Signature", signature);

			return new UriBuilder(Uri.UriSchemeHttp, RequestHost, 80, RequestPath, "?" + collection).Uri.ToString();
		}
		#endregion
	}
}