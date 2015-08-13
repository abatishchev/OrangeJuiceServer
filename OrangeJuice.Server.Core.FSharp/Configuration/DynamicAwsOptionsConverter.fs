namespace OrangeJuice.Server.FSharp.Configuration

open System

open Microsoft.WindowsAzure.Storage.Table

open OrangeJuice.Server
open OrangeJuice.Server.Configuration

type DynamicAwsOptionsConverter() =
    interface IConverter<DynamicTableEntity, AwsOptions> with
        member this.Convert(value : DynamicTableEntity) : AwsOptions =
            new AwsOptions(AssociateTag = value.PartitionKey,
                          AccessKey = value.RowKey,
                          SecretKey = value.["SecretKey"].StringValue,
                          RequestLimit = TimeSpan.Parse(value.["RequestLimit"].StringValue))

        member this.ConvertBack(value : AwsOptions) : DynamicTableEntity =
            let properties = [
                ("SecretKey", new EntityProperty(value.SecretKey));
                ("RequestLimit", new EntityProperty(value.RequestLimit.ToString()))] |> Map.ofSeq
            new DynamicTableEntity(value.AssociateTag,
                                   value.AccessKey,
                                   Properties = properties)
