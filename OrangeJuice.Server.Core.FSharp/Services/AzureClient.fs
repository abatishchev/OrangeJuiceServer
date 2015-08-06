namespace OrangeJuice.Server.FSharp.Services

open System
open System.Threading.Tasks

open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Blob

open OrangeJuice.Server.Configuration
open OrangeJuice.Server.Services

type AzureClient(azureOptions : AzureOptions, blobClient : IBlobClient) =
    static let Year = TimeSpan.FromDays(365.0)

    static let CreateCacheControl = fun (timeSpan : TimeSpan) -> sprintf "public, max-age=%f" timeSpan.TotalMilliseconds
        
    static let CreateFileName = fun (blobName : string) -> sprintf "%s.json" blobName

    let GetContainer = fun (containerName : string) -> async {
        let storageAccount = CloudStorageAccount.Parse(azureOptions.ConnectionString)
        let blobClient = storageAccount.CreateCloudBlobClient()

        let container = blobClient.GetContainerReference(containerName)
        let! exists = container.ExistsAsync() |> Async.AwaitTask
        return match exists with
            | true -> container
            | false -> raise <| new InvalidOperationException(sprintf "Container %s doesn't exist" containerName)
    }

    let GetBlobReference = fun (containerName : string, blobName : string) -> async {
        let! container = GetContainer(containerName)
        let fileName = CreateFileName(blobName)
        return container.GetBlobReferenceFromServer(fileName)
    }
        
    let GetBlockReference = fun (containerName : string, blobName : string) -> async {
        let! container = GetContainer(containerName)
        let fileName = CreateFileName(blobName) 
        return container.GetBlockBlobReference(fileName)
    }
    
    interface IAzureClient with
        member this.GetBlobFromContainer(containerName : string, fileName : string) : Task<string> =
            let task = async {
                let! blob = GetBlockReference(containerName, fileName)
                let! exists = blob.ExistsAsync() |> Async.AwaitTask
                let! content =
                    match exists with
                        | true -> blobClient.Read(blob) |> Async.AwaitTask
                        | false -> Task.FromResult(null) |> Async.AwaitTask
                return content
            }
            task |> Async.StartAsTask

        member this.PutBlobToContainer(containerName : string, fileName : string, content : string) : Task =
            let task = async {
                let! blob = GetBlockReference(containerName, fileName)
                blob.Properties.CacheControl <- CreateCacheControl(Year)
            
                return blobClient.Write(blob, content)
            }
            (task |> Async.StartAsTask) :> Task

        member this.GetBlobUrl(containerName : string, fileName : string) : Task<Uri> =
            let task = async {
                let! blob = GetBlobReference(containerName, fileName)
                return blob.Uri
            }
            task |> Async.StartAsTask
