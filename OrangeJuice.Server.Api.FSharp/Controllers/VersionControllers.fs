namespace OrangeJuice.Server.Api.FSharp.Controllers

open System.Web.Http

open OrangeJuice.Server.Controllers
open OrangeJuice.Server.Data.Models

type VersionController(apiVersion : ApiVersion) =
    inherit ApiController()
    interface IVersionController with
        [<Route("api/version")>]
        member this.GetVersion() : IHttpActionResult  =
            this.Ok(apiVersion) :> IHttpActionResult
