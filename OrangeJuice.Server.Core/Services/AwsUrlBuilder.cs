using System;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public class AwsUrlBuilder : IUrlBuilder
	{
		#region Constants
		private const string RequestHost = "webservices.amazon.com";
		private const string RequestPath = "/onca/xml";
		#endregion

		#region Fields
		private readonly IArgumentBuilder _argumentBuilder;
		private readonly IQueryBuilder _queryBuilder;
		private readonly IQuerySigner _querySigner;
		#endregion

		#region Ctor
		public AwsUrlBuilder(IArgumentBuilder argumentBuilder, IQueryBuilder queryBuilder, IQuerySigner querySigner)
		{
			_argumentBuilder = argumentBuilder;
			_queryBuilder = queryBuilder;
			_querySigner = querySigner;
		}
		#endregion

		#region IUrlBuilder members
		public Uri BuildUrl(AwsProductSearchCriteria searchCriteria)
		{
			var args = _argumentBuilder.BuildArgs(searchCriteria);
			string query = _queryBuilder.BuildQuery(args);
			string signature = _querySigner.CreateSignature(RequestHost, RequestPath, query);
			string signedQuery = _queryBuilder.SignQuery(query, signature);

			return new UriBuilder(Uri.UriSchemeHttp, RequestHost, 80, RequestPath, '?' + signedQuery).Uri;
		}
		#endregion
	}
}