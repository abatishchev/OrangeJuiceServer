namespace OrangeJuice.Server.FSharp.Security

open System
open System.Net.Http
open System.Net.Http.Formatting
open System.Runtime.Serialization
open System.Text.RegularExpressions
open System.Threading.Tasks

open OrangeJuice.Server.Configuration
open OrangeJuice.Server.Data.Models

[<DataContract>]
type AuthTokenRequest = {
    [<field: DataMember(Name="client_id")>]
    client_id : string
    
    [<field: DataMember(Name="access_token")>]
    access_token : string
    
    [<field: DataMember(Name="connection")>]
    connection : string
    
    [<field: DataMember(Name="scope")>]
    scope : string
}

type UnderscoreMappingResolver() =
    inherit Newtonsoft.Json.Serialization.DefaultContractResolver()

    override this.ResolvePropertyName(propertyName : string) : string =
        Regex.Replace(propertyName, "([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", "$1$3_$2$4", RegexOptions.Compiled).ToLower()

type AuthTokenFactory(authOptions : AuthOptions) =
    interface Factory.IFactory<Task<AuthToken>, AuthToken> with
        member this.Create(authorizationToken : AuthToken) : Task<AuthToken> =
            let task = async {
                let httpClient = new HttpClient (
                    BaseAddress = new Uri("https://orangejuice.auth0.com"))
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json") |> ignore

                let request = {
                    client_id = authOptions.Audience;
                    access_token = authorizationToken.AccessToken;
                    connection = "google-oauth2";
                    scope = "openid" }

                let! response = httpClient.PostAsync("oauth/access_token", request, new JsonMediaTypeFormatter()) |> Async.AwaitTask
                response.EnsureSuccessStatusCode() |> ignore
                
                let formatter = new JsonMediaTypeFormatter()
                formatter.SerializerSettings.ContractResolver <- new UnderscoreMappingResolver()

                let! content = response.Content.ReadAsAsync<AuthToken>([| formatter :> MediaTypeFormatter |]) |> Async.AwaitTask
                return content
            }
            task |> Async.StartAsTask