namespace OrangeJuice.Server.FSharp.Configuration

open System.Threading.Tasks 

open OrangeJuice.Server
open OrangeJuice.Server.Configuration
open OrangeJuice.Server.Services 

type AzureAwsOptionsProvider(azureOptions : AzureOptions, azureClient : IAzureClient, converter : IConverter<string, AwsOptions>) =
    interface IOptionsProvider<AwsOptions> with
        member this.GetOptions() : Task<AwsOptions[]> =
            let task = async {
                let! content = azureClient.GetBlobsFromContainer(azureOptions.AwsOptionsContainer) |> Async.AwaitTask
                return Seq.map converter.Convert content
                       |> Array.ofSeq
            }
            task |> Async.StartAsTask
