namespace OrangeJuice.Server.FSharp.Services

open System.Collections.Generic
open System.Threading.Tasks
open System.Xml.Linq

open Factory

open OrangeJuice.Server.Data.Models
open OrangeJuice.Server.Services
open OrangeJuice.Server.Web

type XmlAwsClient(urlBuilder : IUrlBuilder, httpClient : IHttpClient, itemSelector : IItemSelector) =
    interface IAwsClient with
        member this.GetItems(searchCriteria : AwsProductSearchCriteria) : Task<IEnumerable<XElement>> =
            this.GetItems(searchCriteria) |> Async.StartAsTask

    member this.GetItems(searchCriteria : AwsProductSearchCriteria) = async {
        let url = urlBuilder.BuildUrl(searchCriteria)
        let! response = httpClient.GetStringAsync(url) |> Async.AwaitTask
        let items = itemSelector.SelectItems(response)
        return items
    }
