namespace OrangeJuice.Server.FSharp.Services

open System
open System.Threading.Tasks

open Factory

open OrangeJuice.Server.Data
open OrangeJuice.Server.Data.Repository
open OrangeJuice.Server.Services

type CachingCloudProductService(awsProvider : IAwsProductProvider, azureProvider : IAzureProductProvider, productRepository : IProductRepository) =
    interface IProductService with
        member this.Get(productId : Guid) = azureProvider.Get(productId)
        member this.Search(barcode : string, barcodeType : BarcodeType) = this.Search(barcode, barcodeType) |> Async.StartAsTask
        member this.Dispose() =
            productRepository.Dispose()

    member this.Search(barcode : string, barcodeType : BarcodeType) = async {
        let products = productRepository.Search(barcode, barcodeType)
        match products |> List.ofSeq with
            // if empty
            | [] -> let! descriptors = awsProvider.Search(barcode, barcodeType) |> Async.AwaitTask
                    match descriptors |> List.ofSeq with
                        // if empty
                        | [] -> return null
                        // if not empty
                        | _ -> let save = fun (descriptor : ProductDescriptor) -> async {
                                          let! productId = productRepository.Save(barcode, barcodeType) |> Async.AwaitTask
                                          descriptor.ProductId <- productId
                                          azureProvider.Save(descriptor) |> Async.AwaitIAsyncResult |> Async.Ignore |> ignore
                                          return descriptor
                               }
                               let seq = Seq.map (fun d -> save(d) |> Async.StartAsTask) descriptors
                               let! t = Task.WhenAll(seq) |> Async.AwaitTask
                               return t |> Seq.ofArray
            // if not empty
            | _ -> let seq = Seq.map (fun (p : IProduct) -> azureProvider.Get(p.ProductId)) products
                   let! t = Task.WhenAll(seq) |> Async.AwaitTask
                   return t |> Seq.ofArray
    }