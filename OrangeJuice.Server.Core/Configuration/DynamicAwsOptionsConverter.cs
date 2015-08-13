using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace OrangeJuice.Server.Configuration
{
	public sealed class DynamicAwsOptionsConverter : IConverter<DynamicTableEntity, AwsOptions>
	{
		public AwsOptions Convert(DynamicTableEntity value)
		{
			return new AwsOptions
			{
				AssociateTag = value.PartitionKey,
				AccessKey = value.RowKey,
				SecretKey = value["SecretKey"].StringValue,
				RequestLimit = TimeSpan.Parse(value["RequestLimit"].StringValue)
			};
		}

		public DynamicTableEntity ConvertBack(AwsOptions value)
		{
			return new DynamicTableEntity(value.AssociateTag, value.AccessKey)
			{
				Properties = new Dictionary<string, EntityProperty>
				{
					{ "SecretKey", new EntityProperty(value.SecretKey) },
					{ "RequestLimit", new EntityProperty(value.RequestLimit.ToString()) }
				}
			};
		}
	}
}