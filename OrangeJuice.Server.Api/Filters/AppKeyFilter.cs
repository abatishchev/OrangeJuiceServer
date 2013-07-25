using System;

namespace OrangeJuice.Server.Api.Filters
{
	internal sealed class AppKeyFilter : QueryFilterBase
	{
		protected override string QuerySegmentName
		{
			get { return "appKey"; }
		}
	}
}