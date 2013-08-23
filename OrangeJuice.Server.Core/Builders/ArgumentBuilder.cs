using System;
using System.Collections.Generic;

namespace OrangeJuice.Server.Builders
{
	public sealed class ArgumentBuilder
	{
		internal const string AssociateTagKey = "AssociateTag";
		internal const string OperationNameKey = "Operation";

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
				{ AssociateTagKey, _associateTag },
				{ OperationNameKey, operationName }
			};
		}
	}
}