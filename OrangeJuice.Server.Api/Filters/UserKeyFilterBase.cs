namespace OrangeJuice.Server.Api.Filters
{
	internal sealed class UserKeyFilterBase : QueryFilterBase
	{
		protected override string QuerySegmentName
		{
			get { return "userKey"; }
		}
	}
}