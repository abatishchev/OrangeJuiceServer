namespace OrangeJuice.Server.Api.FSharp.Controllers

open System.Web.Http

open OrangeJuice.Server.Controllers
open OrangeJuice.Server.Data.Models

type VersionController(apiVersion : ApiVersion) =
    inherit ApiController()
    interface IVersionController with
        member this.GetVersion() = this.GetVersion();

    [<Route("api/version")>]
    member this.GetVersion() : IHttpActionResult  =
        this.Ok(apiVersion) :> IHttpActionResult
