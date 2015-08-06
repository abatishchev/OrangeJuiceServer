using System;
using System.Collections.Generic;
using System.Linq;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsArgumentBuilder : IArgumentBuilder
	{
		#region Fields

		private readonly AwsOptions _awsOptions;
		private readonly IDateTimeProvider _dateTimeProvider;
		#endregion

		#region Ctor
		public AwsArgumentBuilder(AwsOptions awsOptions, IDateTimeProvider dateTimeProvider)
		{
			_awsOptions = awsOptions;
			_dateTimeProvider = dateTimeProvider;
		}
		#endregion

		#region IArgumentBuilder members
		public IDictionary<string, string> BuildArgs(ProductDescriptorSearchCriteria searchCriteria)
		{
			DateTime now = _dateTimeProvider.GetNow();
			string timestamp = _dateTimeProvider.Format(now);

			// Ordering parameters in naturual byte order as required by AWS
			return new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase)
			{
				{ "Operation", searchCriteria.Operation },
				{ "SearchIndex", searchCriteria.SearchIndex },
				{ "ResponseGroup", String.Join(",", searchCriteria.ResponseGroups ?? Enumerable.Empty<string>()) },
				{ "IdType", searchCriteria.IdType },
				{ "ItemId", searchCriteria.ItemId },
				{ "AWSAccessKeyId", _awsOptions.AccessKey },
				{ "AssociateTag", _awsOptions.AssociateTag },
				{ "Service", "AWSECommerceService" },
				{ "Condition", "All" },
				{ "Timestamp", timestamp }
			};
		}
		#endregion
	}
}