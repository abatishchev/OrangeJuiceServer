using System;
using System.Collections.Generic;

namespace OrangeJuice.Server.Builders
{
	public sealed class ArgumentBuilder
	{
		#region Fields
		internal const string AssociateTagKey = "AssociateTag";

		internal const string ServiceKey = "Service";
		internal const string ServiceValue = "AWSECommerceService";

		internal const string ConditionKey = "Condition";
		internal const string ConditionValue = "All";

		private readonly string _associateTag;
		#endregion

		#region Constructors
		public ArgumentBuilder(string associateTag)
		{
			if (String.IsNullOrEmpty(associateTag))
				throw new ArgumentNullException("associateTag");
			_associateTag = associateTag;
		}
		#endregion

		#region Methods
		public IDictionary<string, string> BuildArgs(IDictionary<string, string> args)
		{
			return new Dictionary<string, string>(args)
			{
				{ServiceKey, ServiceValue},
				{AssociateTagKey, _associateTag},
				{ConditionKey, ConditionValue}
			};
		}
		#endregion
	}
}