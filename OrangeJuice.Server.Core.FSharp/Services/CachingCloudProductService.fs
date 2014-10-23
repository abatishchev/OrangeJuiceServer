namespace OrangeJuice.Server.FSharp.Services

open System
open System.Collections.Generic
open System.Threading.Tasks

open Factory

open OrangeJuice.Server.Data
open OrangeJuice.Server.Data.Models
open OrangeJuice.Server.Services

type CachingCloudProductService(awsProvider : IAwsProductProvider, azureProvider : IAzureProductProvider, productRepository : IProductRepository) =
    interface IProductService with
        member this.Get(productId : Guid) : Task<ProductDescriptor> =
            azureProvider.Get(productId)
        
        member this.Search(barcode : string, barcodeType : BarcodeType) : Task<ProductDescriptor[]> =
            this.Search(barcode, barcodeType) |> Async.StartAsTask
        
        member this.Dispose() : unit =
            productRepository.Dispose()

    member this.Search(barcode : string, barcodeType : BarcodeType) = async {
        let! products = productRepository.Search(barcode, barcodeType) |> Async.AwaitTask
        match products |> List.ofSeq with
            // if empty
            | [] -> let! descriptors = awsProvider.Search(barcode, barcodeType) |> Async.AwaitTask
                    match descriptors |> List.ofSeq with
                        // if empty
                        | [] -> return null
                        // if not empty
                        | _ -> let save = fun () -> async {
                                   for d in descriptors do
                                       let! productId = productRepository.Save(barcode, barcodeType, d.SourceProductId) |> Async.AwaitTask
                                       d.ProductId <- productId
                                       azureProvider.Save(d) |> Async.AwaitIAsyncResult |> Async.Ignore |> ignore
                                   return descriptors
                               }
                               let! t = save() |> Async.StartAsTask |> Async.AwaitTask
                               return t
            // if not empty
            | _ -> let seq = Seq.map (fun (p : Product) -> azureProvider.Get(p.ProductId)) products
                   let! t = Task.WhenAll(seq) |> Async.AwaitTask
                   return t
    }