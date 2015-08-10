namespace OrangeJuice.Server.FSharp.Services

open System
open System.Threading.Tasks

open OrangeJuice.Server.Data
open OrangeJuice.Server.Data.Models
open OrangeJuice.Server.Services

type CachingCloudProductService(awsProvider : IAwsProductProvider, azureProvider : IAzureProductProvider, productRepository : IProductRepository) =

    let save = fun (descriptor : ProductDescriptor, barcode : string, barcodeType : BarcodeType) -> async {
        let! productId = productRepository.Save(barcode, barcodeType, descriptor.SourceProductId) |> Async.AwaitTask
        descriptor.ProductId <- productId
        azureProvider.Save(descriptor) |> Async.AwaitIAsyncResult |> Async.Ignore |> ignore
        return descriptor
    }

    let save = fun (descriptors : ProductDescriptor[], barcode : string, barcodeType : BarcodeType) ->
        Seq.map (fun d -> save(d, barcode, barcodeType)) descriptors |> Async.Parallel

    interface IProductService with
        member this.Get(productId : Guid) : Task<ProductDescriptor> =
            azureProvider.Get(productId)
        
        member this.Search(barcode : string, barcodeType : BarcodeType) : Task<ProductDescriptor[]> =
            let task = async {
                let! products = productRepository.Search(barcode, barcodeType) |> Async.AwaitTask
                match products with
                | [||] -> let! descriptors = awsProvider.Search(barcode, barcodeType) |> Async.AwaitTask
                          match descriptors with
                          | [||] -> return null
                          | _ -> return! save(descriptors, barcode, barcodeType)
                | _ -> return! Seq.map (fun (p : Product) -> azureProvider.Get(p.ProductId)) products |> Task.WhenAll |> Async.AwaitTask
            }
            task |> Async.StartAsTask
