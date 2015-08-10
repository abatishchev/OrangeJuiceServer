namespace OrangeJuice.Server.FSharp.Configuration

open System.Threading.Tasks

open OrangeJuice.Server.Cache
open OrangeJuice.Server.Configuration

type CachingAwsOptionsProvider(optionsProvider : IOptionsProvider<AwsOptions>, cacheClient : ICacheClient) =
    interface IOptionsProvider<AwsOptions> with
        member this.GetOptions() : Task<AwsOptions[]> =
            cacheClient.AddOrGetExisting(AwsOptions.CacheKey, fun() -> optionsProvider.GetOptions())
