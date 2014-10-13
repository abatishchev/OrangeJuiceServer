namespace OrangeJuice.Server.FSharp.Web

open System
open System.Net.Http
open System.Threading.Tasks

open OrangeJuice.Server.Web

type HttpClientAdapter(httpClient : HttpClient) =
    interface IHttpClient with
        member this.GetStringAsync(url : Uri) : Task<string> =
            httpClient.GetStringAsync(url)
