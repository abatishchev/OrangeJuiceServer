namespace OrangeJuice.Server.FSharp.Configuration

open System

open OrangeJuice.Server.Cache
open OrangeJuice.Server.Configuration

type CachingConfigurationProvider(configurationProvider : IConfigurationProvider, cacheClient : ICacheClient) =
    interface IConfigurationProvider with
        member this.GetValue(key : string) : string =
            let valueFactory = new Func<string>(fun() -> configurationProvider.GetValue(key))
            cacheClient.AddOrGetExisting(key, valueFactory)
