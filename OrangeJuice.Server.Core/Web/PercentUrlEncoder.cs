namespace OrangeJuice.Server.Web
{
	public sealed class PercentUrlEncoder : IUrlEncoder
	{
        private readonly IPipeline _pipeline;
        #region Fields
		#endregion

		#region Ctor
		public PercentUrlEncoder(IPipeline pipeline)
		{
			_pipeline = pipeline;
		}
		#endregion

		#region IUrlEncoder members
		public string Encode(string url)
		{
			return (string)_pipeline.Execute(url);
		}
		#endregion
	}
}