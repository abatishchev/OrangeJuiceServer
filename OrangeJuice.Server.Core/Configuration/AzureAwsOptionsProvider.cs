﻿using System.Linq;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Table;

using OrangeJuice.Server.Services;

namespace OrangeJuice.Server.Configuration
{
	public sealed class AzureAwsOptionsProvider : IOptionsProvider<AwsOptions>
	{
		private readonly AzureOptions _azureOptions;
		private readonly IAzureClient _azureClient;
		private readonly IConverter<DynamicTableEntity, AwsOptions> _converter;

		public AzureAwsOptionsProvider(AzureOptions azureOptions, IAzureClient azureClient, IConverter<DynamicTableEntity, AwsOptions> converter)
		{
			_azureOptions = azureOptions;
			_azureClient = azureClient;
			_converter = converter;
		}

		public async Task<AwsOptions[]> GetOptions()
		{
			var content = await _azureClient.GetEntitiesFromTable<DynamicTableEntity>(_azureOptions.AwsOptionsTable);
			return content.Select(_converter.Convert).ToArray();
		}
	}
}