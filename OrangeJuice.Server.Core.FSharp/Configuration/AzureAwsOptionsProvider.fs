namespace OrangeJuice.Server.FSharp.Configuration

open System.Threading.Tasks

open Microsoft.WindowsAzure.Storage.Table

open OrangeJuice.Server
open OrangeJuice.Server.Configuration
open OrangeJuice.Server.Services

type AzureAwsOptionsProvider(azureOptions : AzureOptions, azureClient : IAzureClient, converter : IConverter<DynamicTableEntity, AwsOptions>) =
    interface IOptionsProvider<AwsOptions> with
        member this.GetOptions() : Task<AwsOptions[]> =
            let task = async {
                let! options = azureClient.GetEntitiesFromTable<DynamicTableEntity>(azureOptions.AwsOptionsTable) |> Async.AwaitTask
                return options |> Seq.map converter.Convert
                               |> Array.ofSeq
            }
            task |> Async.StartAsTask
