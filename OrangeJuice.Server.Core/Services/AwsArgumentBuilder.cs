using System;
using System.Collections.Generic;

namespace OrangeJuice.Server.Services
{
	public sealed class AwsArgumentBuilder : IArgumentBuilder
	{
		#region Fields
		private readonly string _accessKey;
		private readonly string _associateTag;
		private readonly IDateTimeProvider _dateTimeProvider;
		#endregion

		#region Constructors
		public AwsArgumentBuilder(string accessKey, string associateTag, IDateTimeProvider dateTimeProvider)
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
				{ "AWSAccessKeyId", _accessKey },
				{ "AssociateTag", _associateTag },
				{ "Service", "AWSECommerceService" },
				{ "Condition", "All" },
				{ "Timestamp", _dateTimeProvider.FormatToUniversal(now) }
			};

			// Use a SortedDictionary to get the parameters in naturual byte order, as required by AWS.
			return new SortedDictionary<string, string>(args, StringComparer.Ordinal);
		}
		#endregion
	}
}