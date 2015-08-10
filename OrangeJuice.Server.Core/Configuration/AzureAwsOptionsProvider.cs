using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Configuration
{
	public sealed class AzureAwsOptionsProvider : IOptionsProvider<AwsOptions>
	{
		private readonly AzureOptions _azureOptions;
		private readonly IAzureClient _azureClient;
		private readonly IConverter<string, AwsOptions> _converter;

		public AzureAwsOptionsProvider(AzureOptions azureOptions, IAzureClient azureClient, IConverter<string, AwsOptions> converter)
		{
			_azureOptions = azureOptions;
			_azureClient = azureClient;
			_converter = converter;
		}

		public async Task<IEnumerable<AwsOptions>> GetOptions()
		{
			var content = await _azureClient.GetBlobsFromContainer(_azureOptions.AwsOptionsContainer);
			return content.Select(_converter.Convert);
		}
	}
}