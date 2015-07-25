namespace OrangeJuice.Server.FSharp.Configuration

open System
open System.Runtime.Caching

open OrangeJuice.Server.Configuration

type CachingConfigurationProvider(configurationProvider : IConfigurationProvider, cache : MemoryCache) =
    interface IConfigurationProvider with
        member this.GetValue(key : string) : string =
            let valueFactory = fun() -> configurationProvider.GetValue(key)
            let newValue = new Lazy<string>(valueFactory)
            let policy = new CacheItemPolicy()
            let value = cache.AddOrGetExisting(key, newValue, policy) :?> Lazy<string>

            let (|??) l r = if l <> null then l else r
            (value |?? newValue).Value
