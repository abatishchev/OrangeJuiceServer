namespace OrangeJuice.Server.Web
{
	public sealed class PercentUrlEncoder : IUrlEncoder
	{
        private readonly IPipeline<string> _pipeline;
        #region Fields
		#endregion

		#region Ctor
		public PercentUrlEncoder(IPipeline<string> pipeline)
		{
			_pipeline = pipeline;
		}
		#endregion

		#region IUrlEncoder members
		public string Encode(string url)
		{
			return _pipeline.Execute(url);
		}
		#endregion
	}
}