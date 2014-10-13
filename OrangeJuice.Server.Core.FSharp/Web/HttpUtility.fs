namespace OrangeJuice.Server.FSharp.Web

open System.Collections.Specialized

open OrangeJuice.Server.Web

type HttpUtility() =
    static member ParseQueryString(query : string, urlEncoder : IUrlEncoder) : NameValueCollection =
        let coll = System.Web.HttpUtility.ParseQueryString(query)
        new QueryStringNameValueCollection(coll, urlEncoder) :> NameValueCollection
