﻿namespace OrangeJuice.Server.FSharp.Services

open System.Threading.Tasks
open System.Xml.Linq

open Factory

open OrangeJuice.Server.Data
open OrangeJuice.Server.Services
open OrangeJuice.Server.Web

type XmlAwsClient(urlBuilder : IUrlBuilder, httpClient : IHttpClient, itemSelector : IItemSelector, factory : IFactory<ProductDescriptor, XElement>) =
    interface IAwsClient with
        member this.GetItems(searchCriteria : ProductDescriptorSearchCriteria) = this.GetItems(searchCriteria) |> Async.StartAsTask

    member this.GetItems(searchCriteria : ProductDescriptorSearchCriteria) = async {
        let url = urlBuilder.BuildUrl(searchCriteria)
        let! response = httpClient.GetStringAsync(url) |> Async.AwaitTask
        let items = itemSelector.SelectItems(response)
        let arr = Seq.map factory.Create items
        return arr
    }