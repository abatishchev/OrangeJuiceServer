namespace OrangeJuice.Server.FSharp.Services

open System
open System.Threading.Tasks

open OrangeJuice.Server
open OrangeJuice.Server.Configuration
open OrangeJuice.Server.Data.Models
open OrangeJuice.Server.Services

type AzureProductProvider(azureOptions : AzureOptions, client : IAzureClient, converter : IConverter<string, ProductDescriptor>) =
    interface IAzureProductProvider with
        member this.Get(productId : Guid) : Task<ProductDescriptor> =
            let task = async {
                let! content = client.GetBlobFromContainer(azureOptions.ProductsContainer, productId.ToString()) |> Async.AwaitTask
                return if content <> null 
                    then converter.Convert(content)
                    else null
            }
            task |> Async.StartAsTask

        member this.Save(descriptor : ProductDescriptor) : Task =
            let content = converter.ConvertBack(descriptor)
            client.PutBlobToContainer(azureOptions.ProductsContainer, descriptor.ProductId.ToString(), content)
        
        member this.GetUrl(productId : Guid) : Task<Uri> =
            client.GetBlobUrl(azureOptions.ProductsContainer, productId.ToString())
