using System;

namespace OrangeJuice.Server.Web
{
	public sealed class PercentUrlEncoder : IUrlEncoder
	{
		private readonly IPipeline<string> _encodingPipeline;

		#region Fields
		#endregion

		#region Ctor
		public PercentUrlEncoder(IPipeline<string> encodingPipeline)
		{
			if (encodingPipeline == null)
				throw new ArgumentNullException("encodingPipeline");

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