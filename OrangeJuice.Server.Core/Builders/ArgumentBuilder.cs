using System;
using System.Collections.Generic;

namespace OrangeJuice.Server.Builders
{
	public sealed class ArgumentBuilder
	{
		internal const string AssociateTagKey = "AssociateTag";
		internal const string OperationNameKey = "Operation";

		internal const string ServiceKey = "Service";
		internal const string ServiceName = "AWSECommerceService";

		private readonly string _associateTag;

		public ArgumentBuilder(string associateTag)
		{
			if (String.IsNullOrEmpty(associateTag))
				throw new ArgumentNullException("associateTag");
			_associateTag = associateTag;
		}

		public IDictionary<string, string> BuildArgs(IDictionary<string, string> args, string operationName)
		{
			return new Dictionary<string, string>(args)
			{
				{ ServiceKey, ServiceName },
				{ AssociateTagKey, _associateTag },
				{ OperationNameKey, operationName }
			};
		}
	}
}