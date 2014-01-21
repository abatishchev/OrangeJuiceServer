using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsArgumentBuilder : IArgumentBuilder
	{
		#region Fields
		private readonly string _accessKey;
		private readonly string _associateTag;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly IUrlEncoder _urlEncoder;
		#endregion

		#region Ctor
		public AwsArgumentBuilder(string accessKey, string associateTag, IDateTimeProvider dateTimeProvider, IUrlEncoder urlEncoder)
		{
			_accessKey = accessKey;
			_associateTag = associateTag;
			_dateTimeProvider = dateTimeProvider;
			_urlEncoder = urlEncoder;
		}
		#endregion

		#region IArgumentBuilder members
		public NameValueCollection BuildArgs(IDictionary<string, string> args)
		{
			DateTime now = _dateTimeProvider.GetNow();
			string timestamp = _dateTimeProvider.FormatToUniversal(now);

			args = new Dictionary<string, string>(args)
			{
				{ "AWSAccessKeyId", _accessKey },
				{ "AssociateTag", _associateTag },
				{ "Service", "AWSECommerceService" },
				{ "Condition", "All" },
				{ "Timestamp", timestamp }
			};

			// Use a SortedDictionary to get the parameters in naturual byte order, as required by AWS.
			args = new SortedDictionary<string, string>(args, StringComparer.Ordinal);

			NameValueCollection collection = new NameValueCollection(args.Count);
			foreach (var p in args)
			{
				collection.Add(p.Key, _urlEncoder.Encode(p.Value));
			}
			return collection;
		}
		#endregion
	}
}