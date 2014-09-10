namespace OrangeJuice.Server.Web
{
	public sealed class PercentUrlEncoder : IUrlEncoder
	{
		private readonly IPipeline<string, string> _encodingPipeline;

		#region Fields
		#endregion

		#region Ctor
		public PercentUrlEncoder(IPipeline<string, string> encodingPipeline)
		{
			_encodingPipeline = encodingPipeline;
		}
		#endregion

		#region IUrlEncoder members
		public string Encode(string url)
		{
			return _encodingPipeline.Run(url);
		}
		#endregion
	}
}