namespace OrangeJuice.Server.FSharp.Cache

open System
open System.Runtime.Caching

open OrangeJuice.Server.Cache

type MemoryCacheClient(cache : ObjectCache) =
    interface ICacheClient with
        member this.AddOrGetExisting<'T>(key : string, valueFactory : Func<'T>) : 'T =
           (this :> ICacheClient).AddOrGetExisting(key, valueFactory, fun () -> new CacheItemPolicy())
        
        member this.AddOrGetExisting<'T>(key : string, valueFactory : Func<'T>, policyFactory : Func<CacheItemPolicy>) : 'T =
            let newValue = new Lazy<'T>(valueFactory)
            let policy = policyFactory.Invoke()
            let value = cache.AddOrGetExisting(key, newValue, policy) :?> Lazy<'T>

            let (|??) l r = if l <> null then l else r
            (value |?? newValue).Value
