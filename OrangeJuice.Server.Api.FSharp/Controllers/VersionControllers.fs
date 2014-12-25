namespace OrangeJuice.Server.Api.FSharp.Controllers

open System.Web.Http

open OrangeJuice.Server.Data.Models

type VersionController(apiVersion : ApiVersion) =
    inherit ApiController()

    [<Route("api/version")>]
    member this.GetVersion() : IHttpActionResult  =
        this.Ok(apiVersion) :> IHttpActionResult
