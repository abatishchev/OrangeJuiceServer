namespace OrangeJuice.Server.Api.FSharp.Controllers

open System
open System.Threading.Tasks
open System.Web.Http

open OrangeJuice.Server.Controllers
open OrangeJuice.Server.Data.Models
open OrangeJuice.Server.Services

[<Authorize>]
type ProductController(productService : IProductService) =
    inherit ApiController()
    interface IProductController with
        member this.GetProductId(searchCriteria : ProductSearchCriteria) =
            this.GetProductId(searchCriteria)

        member this.GetProductBarcode(searchCriteria : BarcodeSearchCriteria) =
            this.GetProductBarcode(searchCriteria)

    member this.Ok<'T>(args : 'T) =  
        base.Ok(args)

    [<Route("api/product/id")>]
    member this.GetProductId([<FromUri>] searchCriteria : ProductSearchCriteria) : Task<IHttpActionResult> =
        if (searchCriteria = null) then raise <| new ArgumentNullException()

        let task = async {
            let! descriptor = productService.Get(searchCriteria.ProductId) |> Async.AwaitTask
            return
                match descriptor with
                | null -> this.NoContent()
                | _ -> this.Ok(descriptor) :> IHttpActionResult
        }
        task |> Async.StartAsTask

    [<Route("api/product/barcode")>]
    member this.GetProductBarcode([<FromUri>] searchCriteria : BarcodeSearchCriteria) : Task<IHttpActionResult>  =
        if (searchCriteria = null) then raise <| new ArgumentNullException()

        let task = async {
            let! descriptor = productService.Search(searchCriteria.Barcode, searchCriteria.BarcodeType) |> Async.AwaitTask
            return match descriptor with
                | null -> this.NoContent()
                | _ -> this.Ok(descriptor) :> IHttpActionResult
        }
        task |> Async.StartAsTask
