namespace OrangeJuice.Server.Api.Filters
{
	internal sealed class UserKeyFilter : QueryFilterBase
	{
		protected override string QuerySegmentName
		{
			get { return "userKey"; }
		}
	}
}