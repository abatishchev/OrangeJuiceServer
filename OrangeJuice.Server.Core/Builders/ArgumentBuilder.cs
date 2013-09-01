using System;
using System.Collections.Generic;

namespace OrangeJuice.Server.Builders
{
	public sealed class ArgumentBuilder : IArgumentBuilder
	{
		#region Constants
		internal const string AwsAccessKey = "AWSAccessKeyId";
		internal const string AssociateTagKey = "AssociateTag";

		internal const string ServiceKey = "Service";
		internal const string ServiceValue = "AWSECommerceService";

		internal const string ConditionKey = "Condition";
		internal const string ConditionValue = "All";

		internal const string TimestampKey = "Timestamp";
		#endregion

		#region Fields
		private readonly string _accessKey;
		private readonly string _associateTag;
		private readonly IDateTimeProvider _dateTimeProvider;
		#endregion

		#region Constructors
		public ArgumentBuilder(string accessKey, string associateTag, IDateTimeProvider dateTimeProvider)
		{
			if (String.IsNullOrEmpty(accessKey))
				throw new ArgumentNullException("accessKey");
			if (String.IsNullOrEmpty(associateTag))
				throw new ArgumentNullException("associateTag");
			if (dateTimeProvider == null)
				throw new ArgumentNullException("dateTimeProvider");

			_accessKey = accessKey;
			_associateTag = associateTag;
			_dateTimeProvider = dateTimeProvider;
		}
		#endregion

		#region Methods
		public IDictionary<string, string> BuildArgs(IDictionary<string, string> args)
		{
			DateTime now = _dateTimeProvider.GetNow();

			args = new Dictionary<string, string>(args)
			{
				{ AwsAccessKey, _accessKey },
				{ AssociateTagKey, _associateTag },
				{ ServiceKey, ServiceValue },
				{ ConditionKey, ConditionValue },
				{ TimestampKey, _dateTimeProvider.FormatToUniversal(now) }
			};

			// Use a SortedDictionary to get the parameters in naturual byte order, as required by AWS.
			return new SortedDictionary<string, string>(args, StringComparer.Ordinal);
		}
		#endregion
	}
}