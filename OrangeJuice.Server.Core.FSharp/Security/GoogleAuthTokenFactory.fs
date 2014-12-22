namespace OrangeJuice.Server.FSharp.Security

open System
open System.Collections.Generic
open System.Net.Http
open System.Net.Http.Formatting
open System.Threading.Tasks

open Factory

open OrangeJuice.Server.Data.Models

type GoogleAuthTokenFactory(jwtFactory : IFactory<string>) =
    interface IFactory<Task<AuthToken>, string> with
        member this.Create(authorizationToken : string) : Task<AuthToken> =
            let task = async {
                let jwt = jwtFactory.Create()

                let dic = dict [ ("grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer"); ("assertion", jwt) ]

                let content = new FormUrlEncodedContent(dic)

                let httpClient = new HttpClient (
                    BaseAddress = new Uri("https://accounts.google.com"))
                
                let! response = httpClient.PostAsync("/o/oauth2/token", content) |> Async.AwaitTask
                response.EnsureSuccessStatusCode() |> ignore

                let formatter = new JsonMediaTypeFormatter()
                formatter.SerializerSettings.ContractResolver <- new UnderscoreMappingResolver()

                let! content = response.Content.ReadAsAsync<AuthToken>([| formatter :> MediaTypeFormatter |]) |> Async.AwaitTask
                return content
            }
            task |> Async.StartAsTask
