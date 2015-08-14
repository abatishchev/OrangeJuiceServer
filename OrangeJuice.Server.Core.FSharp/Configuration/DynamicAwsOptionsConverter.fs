namespace OrangeJuice.Server.FSharp.Configuration

open System
open System.Globalization

open Microsoft.WindowsAzure.Storage.Table

open OrangeJuice.Server
open OrangeJuice.Server.Configuration

type DynamicAwsOptionsConverter() =
    [<Literal>] 
    let TimeSpanFormat = @"hh\:mm\:ss\.fff"

    interface IConverter<DynamicTableEntity, AwsOptions> with
        member this.Convert(value : DynamicTableEntity) : AwsOptions =
            new AwsOptions(AssociateTag = value.PartitionKey,
                          AccessKey = value.RowKey,
                          SecretKey = value.["SecretKey"].StringValue,
                          RequestLimit = TimeSpan.ParseExact(value.["RequestLimit"].StringValue, TimeSpanFormat, CultureInfo.InvariantCulture))

        member this.ConvertBack(value : AwsOptions) : DynamicTableEntity =
            let properties = [
                ("SecretKey", new EntityProperty(value.SecretKey));
                ("RequestLimit", new EntityProperty(value.RequestLimit.ToString(TimeSpanFormat)))] |> Map.ofSeq
            new DynamicTableEntity(value.AssociateTag,
                                   value.AccessKey,
                                   Properties = properties)
