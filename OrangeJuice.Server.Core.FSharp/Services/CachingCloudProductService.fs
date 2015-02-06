namespace OrangeJuice.Server.FSharp.Services

open System
open System.Collections.Generic
open System.Threading.Tasks

open Factory

open OrangeJuice.Server.Data
open OrangeJuice.Server.Data.Models
open OrangeJuice.Server.Services

type CachingCloudProductService(awsProvider : IAwsProductProvider, azureProvider : IAzureProductProvider, productRepository : IProductRepository) =
    let save = fun (descriptors : ProductDescriptor[], barcode : string, barcodeType : BarcodeType) -> async {
        for d in descriptors do
            let! productId = productRepository.Save(barcode, barcodeType, d.SourceProductId) |> Async.AwaitTask
            d.ProductId <- productId
            azureProvider.Save(d) |> Async.AwaitIAsyncResult |> Async.Ignore |> ignore
        return descriptors
    }

    interface IProductService with
        member this.Get(productId : Guid) : Task<ProductDescriptor> =
            azureProvider.Get(productId)
        
        member this.Search(barcode : string, barcodeType : BarcodeType) : Task<ProductDescriptor[]> =
            this.Search(barcode, barcodeType) |> Async.StartAsTask

    member this.Search(barcode : string, barcodeType : BarcodeType) = async {
        let! products = productRepository.Search(barcode, barcodeType) |> Async.AwaitTask
        match products |> List.ofSeq with
            | [] -> let! descriptors = awsProvider.Search(barcode, barcodeType) |> Async.AwaitTask
                    match descriptors |> List.ofSeq with
                        | [] -> return null
                        | _ -> let! t = save(descriptors, barcode, barcodeType) |> Async.StartAsTask |> Async.AwaitTask
                               return t
            | _ -> let seq = Seq.map (fun (p : Product) -> azureProvider.Get(p.ProductId)) products
                   let! t = Task.WhenAll(seq) |> Async.AwaitTask
                   return t
    }