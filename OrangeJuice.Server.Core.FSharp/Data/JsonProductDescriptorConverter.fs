namespace OrangeJuice.Server.FSharp.Data

open Newtonsoft.Json
open Newtonsoft.Json.Linq

open OrangeJuice.Server
open OrangeJuice.Server.Data

type JsonProductDescriptorConverter() =
    interface IConverter<string, ProductDescriptor> with
        member this.Convert(value : string) : ProductDescriptor =
            JObject.Parse(value).ToObject<ProductDescriptor>()
        member this.ConvertBack(value : ProductDescriptor) : string =
            JObject.FromObject(value).ToString(Formatting.Indented)
