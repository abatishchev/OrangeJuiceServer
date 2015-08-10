namespace OrangeJuice.Server.FSharp.Configuration

open Newtonsoft.Json
open Newtonsoft.Json.Linq

open OrangeJuice.Server
open OrangeJuice.Server.Configuration

type JsonAwsOptionsConverter() =
    interface IConverter<string, AwsOptions> with
        member this.Convert(value : string) : AwsOptions =
            JObject.Parse(value).ToObject<AwsOptions>()

        member this.ConvertBack(value : AwsOptions) : string =
            JObject.FromObject(value).ToString(Formatting.None)
