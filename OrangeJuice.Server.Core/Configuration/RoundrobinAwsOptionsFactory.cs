using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Configuration
{
	public class RoundrobinAwsOptionsFactory : Factory.IFactory<AwsOptions>
	{
		private readonly Lazy<IEnumerator<AwsOptions>> _e;

		public RoundrobinAwsOptionsFactory(IOptionsProvider<AwsOptions> optionsProvider)
		{
			_e = new Lazy<IEnumerator<AwsOptions>>(() => GetEnumerator(optionsProvider));
		}

		private static IEnumerator<AwsOptions> GetEnumerator(IOptionsProvider<AwsOptions> optionsProvider)
		{
			var options = Task.Run(async () => await optionsProvider.GetOptions()).Result;
			return options.AsInfinite().GetEnumerator();
		}

		public AwsOptions Create()
		{
			var e = _e.Value;
			e.MoveNext();
			return e.Current;
		}
	}
}