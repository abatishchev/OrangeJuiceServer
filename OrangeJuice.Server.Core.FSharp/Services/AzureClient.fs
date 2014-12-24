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

    let GetContainer = fun (containerName : string) ->
        let storageAccount = CloudStorageAccount.Parse(azureOptions.ConnectionString)
        let blobClient = storageAccount.CreateCloudBlobClient()

        let container = blobClient.GetContainerReference(containerName)
        match container.Exists() with
            | true -> container
            | false -> raise <| new InvalidOperationException(sprintf "Container %s doesn't exist" containerName)

    let GetBlobReference = fun (containerName : string, blobName : string) ->
        let container = GetContainer(containerName)
        let fileName = CreateFileName(blobName)
        container.GetBlobReferenceFromServer(fileName)
        
    let GetBlockReference = fun (containerName : string, blobName : string) ->
        let container = GetContainer(containerName)
        let fileName = CreateFileName(blobName)
        container.GetBlockBlobReference(fileName)
    
    interface IAzureClient with
        member this.GetBlobFromContainer(containerName : string, fileName : string) : Task<string> =
            let task = async {
                let blob = GetBlockReference(containerName, fileName)
                let! content =
                    match blob.Exists() with
                        | true -> blobClient.Read(blob) |> Async.AwaitTask
                        | false -> Task.FromResult(null) |> Async.AwaitTask
                return content
            }
            task |> Async.StartAsTask

        member this.PutBlobToContainer(containerName : string, fileName : string, content : string) : Task =
            let blob = GetBlockReference(containerName, fileName)
            blob.Properties.CacheControl <- CreateCacheControl(Year)
            
            blobClient.Write(blob, content)

        member this.GetBlobUrl(containerName : string, fileName : string) : Uri =
            let blob = GetBlobReference(containerName, fileName)
            blob.Uri

