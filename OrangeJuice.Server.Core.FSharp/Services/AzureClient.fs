namespace OrangeJuice.Server.FSharp.Services

open System
open System.Threading.Tasks

open Microsoft.WindowsAzure.Storage.Blob

open OrangeJuice.Server.Services

type AzureClient(blobClient : IBlobClient, containerClient : IAzureContainerClient) =
    static let Year = TimeSpan.FromDays(365.0)

    static let CreateCacheControl = fun (timeSpan : TimeSpan) -> sprintf "public, max-age=%f" timeSpan.TotalMilliseconds
        
    interface IAzureClient with
        member this.GetBlobFromContainer(containerName : string, fileName : string) : Task<string> =
            let task = async {
                let! blob = containerClient.GetBlockReference(containerName, fileName) |> Async.AwaitTask
                let! exists = blob.ExistsAsync() |> Async.AwaitTask
                let! content =
                    match exists with
                        | true -> blobClient.Read(blob) |> Async.AwaitTask
                        | false -> Task.FromResult(null) |> Async.AwaitTask
                return content
            }
            task |> Async.StartAsTask 

        member this.GetBlobsFromContainer(containerName : string) : Task<string[]> =
            let task = async {
                let! container = containerClient.GetContainer(containerName) |> Async.AwaitTask
                return! container.ListBlobs()
                        |> Seq.cast<CloudBlockBlob>
                        |> Seq.map (fun b -> b.DownloadTextAsync() |> Async.AwaitTask)
                        |> Async.Parallel
            }   
            task |> Async.StartAsTask

        member this.PutBlobToContainer(containerName : string, fileName : string, content : string) : Task =
            let task = async {
                let! blob = containerClient.GetBlockReference(containerName, fileName) |> Async.AwaitTask
                blob.Properties.CacheControl <- CreateCacheControl(Year)
            
                return blobClient.Write(blob, content)
            }
            (task |> Async.StartAsTask) :> Task

        member this.GetBlobUrl(containerName : string, fileName : string) : Task<Uri> =
            let task = async {
                let! blob = containerClient.GetBlobReference(containerName, fileName) |> Async.AwaitTask
                return blob.Uri
            }
            task |> Async.StartAsTask
