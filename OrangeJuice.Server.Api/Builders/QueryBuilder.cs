using System;
using System.Collections.Generic;
using System.Linq;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Api.Builders
{
	internal class QueryBuilder
	{
		private readonly string _accessKey;
		private readonly IUrlEncoder _urlEncoder;
		private readonly IDateTimeProvider _dateTimeProvider;

		public QueryBuilder(string accessKey, IUrlEncoder urlEncoder, IDateTimeProvider dateTimeProvider)
		{
			_accessKey = accessKey;
			_urlEncoder = urlEncoder;
			_dateTimeProvider = dateTimeProvider;
		}

		public string BuildQuery(IDictionary<string, string> dic)
		{
			// Use a SortedDictionary to get the parameters in naturual byte order, as required by AWS.
			dic = new SortedDictionary<string, string>(dic, StringComparer.Ordinal);

			// Add the AWSAccessKeyId and Timestamp to the requests.
			dic["AWSAccessKeyId"] = _accessKey;
			dic["Timestamp"] = _dateTimeProvider.FormatToUniversal(DateTime.UtcNow);

			// Get the canonical query string
			return String.Join("&",
				dic.Select(p => String.Format("{0}={1}",
					_urlEncoder.Encode(p.Key),
					_urlEncoder.Encode(p.Value))));
		}
	}
}