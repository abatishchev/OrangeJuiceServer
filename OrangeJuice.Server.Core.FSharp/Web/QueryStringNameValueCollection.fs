namespace OrangeJuice.Server.FSharp.Web

open System
open System.Collections.Generic
open System.Collections.Specialized
open System.Linq

open OrangeJuice.Server.Web
 
type QueryStringNameValueCollection(coll : NameValueCollection, urlEncoder : IUrlEncoder) =
    inherit NameValueCollection(coll)

    override this.Get(name : string) : string =
        let value = base.Get(name)
        if value <> null
            then value
            else String.Empty

    override this.ToString() : string =
        let args = this.AllKeys |>
                   Seq.map (fun k -> new KeyValuePair<string, string>(k, urlEncoder.Encode(this.Item(k)))) |>
                   Seq.map (fun p -> sprintf "%s=%s" p.Key p.Value)
        String.Join("&", args) 
