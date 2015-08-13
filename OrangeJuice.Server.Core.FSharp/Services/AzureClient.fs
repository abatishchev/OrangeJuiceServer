namespace OrangeJuice.Server.FSharp.Services

open System
open System.Collections.Generic
open System.Threading.Tasks

open Microsoft.WindowsAzure.Storage.Blob
open Microsoft.WindowsAzure.Storage.Table;

open OrangeJuice.Server.Services

type AzureClient(blobClient : IBlobClient, containerClient : IAzureContainerClient) =
    static let Year = TimeSpan.FromDays(365.0)

    static let CreateCacheControl = fun (timeSpan : TimeSpan) -> sprintf "public, max-age=%f" timeSpan.TotalMilliseconds

    interface IAzureClient with
        member this.GetBlobFromContainer(containerName : string, fileName : string) : Task<string> =
            let task = async {
                let! blob = containerClient.GetBlockReference(containerName, fileName) |> Async.AwaitTask
                let! exists = blob.ExistsAsync() |> Async.AwaitTask
                return!
                    match exists with
                        | true -> blobClient.Read(blob) |> Async.AwaitTask
                        | false -> Task.FromResult(null) |> Async.AwaitTask
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
            task |> Async.StartAsTask :> Task

        member this.GetBlobUrl(containerName : string, fileName : string) : Task<Uri> =
            let task = async {
                let! blob = containerClient.GetBlobReference(containerName, fileName) |> Async.AwaitTask
                let! exists = blob.ExistsAsync() |> Async.AwaitTask
                return
                    match exists with
                        | true -> blob.Uri
                        | false -> null
            }
            task |> Async.StartAsTask

        member this.GetEntitiesFromTable<'T when 'T :> ITableEntity  and 'T : (new : unit -> 'T)>(tableName : string) : Task<'T[]> =
            let task = async {
                let! table = containerClient.GetTableReference(tableName) |> Async.AwaitTask
                let query = new TableQuery<'T>();
                return table.ExecuteQuery(query)
                       |> Array.ofSeq
            }
            task |> Async.StartAsTask

        member this.PutEntitiesToTable<'T when 'T :> ITableEntity>(tableName : string, entities : seq<'T>) : Task<IList<TableResult>> =
            let task = async {
                let! table = containerClient.GetTableReference(tableName) |> Async.AwaitTask
                let batch = new TableBatchOperation()
                for e in entities do
                    batch.Insert(e)
                return! table.ExecuteBatchAsync(batch) |> Async.AwaitTask
            }
            task |> Async.StartAsTask