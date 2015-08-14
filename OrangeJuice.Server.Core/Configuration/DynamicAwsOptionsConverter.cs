using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.WindowsAzure.Storage.Table;

namespace OrangeJuice.Server.Configuration
{
	public sealed class DynamicAwsOptionsConverter : IConverter<DynamicTableEntity, AwsOptions>
	{
		private const string TimeSpanFormat = @"hh\:mm\:ss\.fff";

		public AwsOptions Convert(DynamicTableEntity value)
		{
			return new AwsOptions
			{
				AssociateTag = value.PartitionKey,
				AccessKey = value.RowKey,
				SecretKey = value["SecretKey"].StringValue,
				RequestLimit = TimeSpan.ParseExact(value["RequestLimit"].StringValue, TimeSpanFormat, CultureInfo.InvariantCulture)
			};
		}

		public DynamicTableEntity ConvertBack(AwsOptions value)
		{
			return new DynamicTableEntity(value.AssociateTag, value.AccessKey)
			{
				Properties = new Dictionary<string, EntityProperty>
				{
					{ "SecretKey", new EntityProperty(value.SecretKey) },
					{ "RequestLimit", new EntityProperty(value.RequestLimit.ToString(TimeSpanFormat)) }
				}
			};
		}
	}
}