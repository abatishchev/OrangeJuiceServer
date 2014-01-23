﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsArgumentBuilder : IArgumentBuilder
	{
		#region Fields
		private readonly string _accessKey;
		private readonly string _associateTag;
		private readonly IDateTimeProvider _dateTimeProvider;
		#endregion

		#region Ctor
		public AwsArgumentBuilder(string accessKey, string associateTag, IDateTimeProvider dateTimeProvider)
		{
			_accessKey = accessKey;
			_associateTag = associateTag;
			_dateTimeProvider = dateTimeProvider;
		}
		#endregion

		#region IArgumentBuilder members
		public IDictionary<string, string> BuildArgs(IDictionary<string, string> args)
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

			return args.OrderBy(p => p.Key)
					   .ToDictionary(p => p.Key, p => p.Value);
		}
		#endregion
	}
}