namespace OrangeJuice.Server.FSharp

open System.Xml.Linq

open Factory

open OrangeJuice.Server.Data
open OrangeJuice.Server.Services
open OrangeJuice.Server.Web

type XmlAwsClientPipeline(urlBuilder : IUrlBuilder, httpClient : IHttpClient, itemSelector : IItemSelector, factory : IFactory<ProductDescriptor, XElement>) =
    member this.Execute(searchCriteria : ProductDescriptorSearchCriteria) =
        let a = async {
            let url = urlBuilder.BuildUrl searchCriteria
            let response = httpClient.GetStringAsync url
            let items = itemSelector.SelectItems response 
            let seq = Seq.map items factory.Create
            return seq
        }
