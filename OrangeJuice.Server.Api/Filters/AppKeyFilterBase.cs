using System;

namespace OrangeJuice.Server.Api.Filters
{
	internal sealed class AppKeyFilterBase : QueryFilterBase
	{
		protected override string QuerySegmentName
		{
			get { return "appKey"; }
		}
	}
}