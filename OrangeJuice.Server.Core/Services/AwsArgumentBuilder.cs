using System;
using System.Collections.Generic;

using OrangeJuice.Server.Configuration;

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
		public AwsArgumentBuilder(AwsOptions awsOptions, IDateTimeProvider dateTimeProvider)
		{
			_accessKey = awsOptions.AccessKey;
			_associateTag = awsOptions.AssociateTag;
			_dateTimeProvider = dateTimeProvider;
		}
		#endregion

		#region IArgumentBuilder members
		public IDictionary<string, string> BuildArgs(Data.ProductDescriptorSearchCriteria searchCriteria)
		{
			DateTime now = _dateTimeProvider.GetNow();
			string timestamp = _dateTimeProvider.Format(now);

			// Ordering parameters in naturual byte order as required by AWS
			return new SortedDictionary<string, string>(StringComparer.Ordinal)
			{
				{ "Operation", searchCriteria.Operation },
				{ "SearchIndex", searchCriteria.SearchIndex },
				{ "ResponseGroup", String.Join(",", searchCriteria.ResponseGroups) },
				{ "IdType", searchCriteria.IdType },
				{ "ItemId", searchCriteria.ItemId },
				{ "AWSAccessKeyId", _accessKey },
				{ "AssociateTag", _associateTag },
				{ "Service", "AWSECommerceService" },
				{ "Condition", "All" },
				{ "Timestamp", timestamp }
			};
		}
		#endregion
	}
}