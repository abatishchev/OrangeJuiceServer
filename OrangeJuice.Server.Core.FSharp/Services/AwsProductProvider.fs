namespace OrangeJuice.Server.FSharp.Services

open System.Threading.Tasks
open System.Xml.Linq

open Factory

open OrangeJuice.Server.Data.Models
open OrangeJuice.Server.Services

type AwsProductProvider(client : IAwsClient, factory : IFactory<ProductDescriptor, XElement, AwsProductSearchCriteria>) =
    interface IAwsProductProvider with
        member this.Search(barcode : string, barcodeType : BarcodeType) : Task<ProductDescriptor[]> =
            this.Search(barcode, barcodeType) |> Async.StartAsTask

    member this.Search(barcode : string, barcodeType : BarcodeType) = async {
        let searchCriteria = new AwsProductSearchCriteria(
            Operation = "ItemLookup",
            SearchIndex = "Grocery",
            ResponseGroups = [| "Images"; "ItemAttributes" |],
            IdType = barcodeType.ToString(),
            ItemId = barcode)

        let! items = client.GetItems(searchCriteria) |> Async.AwaitTask
        let seq = Seq.map (fun item -> factory.Create(item, searchCriteria)) items
        return seq |> Array.ofSeq
    }