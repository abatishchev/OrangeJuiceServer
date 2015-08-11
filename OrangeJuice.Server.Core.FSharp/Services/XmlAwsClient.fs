namespace OrangeJuice.Server.FSharp.Services

open System.Threading.Tasks
open System.Xml.Linq

open OrangeJuice.Server.Data.Models
open OrangeJuice.Server.Filters
open OrangeJuice.Server.Services
open OrangeJuice.Server.Web

type XmlAwsClient(urlBuilder : IUrlBuilder, httpClient : IHttpClient, itemSelector : IItemSelector, itemFilter : IFilter<XElement>) =
    interface IAwsClient with
        member this.GetItems(searchCriteria : AwsProductSearchCriteria) : Task<XElement[]> =
            let task = async {
                let url = urlBuilder.BuildUrl(searchCriteria)
                let! response = httpClient.GetStringAsync(url) |> Async.AwaitTask
                let items = itemSelector.SelectItems(response)
                return if items.Length > 1
                    then items |> Array.filter itemFilter.Filter
                    else items
            }
            task |> Async.StartAsTask
