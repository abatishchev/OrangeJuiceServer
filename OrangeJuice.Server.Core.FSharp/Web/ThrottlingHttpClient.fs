namespace OrangeJuice.Server.FSharp.Web

open System
open System.Threading.Tasks

open OrangeJuice.Server.Web
open OrangeJuice.Server.Threading

type ThrottlingHttpClient(httpClient : IHttpClient, scheduler : IRequestScheduler) =
    interface IHttpClient with
        member this.GetStringAsync(url : Uri) : Task<string> =
            scheduler.ScheduleRequest(fun () -> httpClient.GetStringAsync(url))
