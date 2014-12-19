namespace OrangeJuice.Server.FSharp.Web

open OrangeJuice.Server.Web

open System
open System.Collections.Generic

type EncodedQueryBuilder(urlEncoder : IUrlEncoder) =
    interface IQueryBuilder with
        member this.BuildQuery(args : IEnumerable<KeyValuePair<string, string>>) : string =
            let coll = HttpUtility.ParseQueryString(String.Empty, urlEncoder)
            args |> Seq.iter (fun arg -> coll.Add(arg.Key, arg.Value))
            coll.ToString()

        member this.SignQuery(query : string, signature : string) =
            let coll = HttpUtility.ParseQueryString(query, urlEncoder)
            coll.Add("Signature", signature) |> ignore
            coll.ToString()
