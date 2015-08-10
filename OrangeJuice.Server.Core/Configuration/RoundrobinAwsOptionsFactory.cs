using System.Collections.Generic;

namespace OrangeJuice.Server.Configuration
{
	public class RoundrobinAwsOptionsFactory : Factory.IFactory<AwsOptions>
	{
		private readonly IEnumerator<AwsOptions> _e;

		public RoundrobinAwsOptionsFactory(IEnumerable<AwsOptions> options)
		{
			_e = options.AsInfinite().GetEnumerator();
		}

		public AwsOptions Create()
		{
			_e.MoveNext();
			return _e.Current;
		}
	}
}