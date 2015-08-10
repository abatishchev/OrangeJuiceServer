namespace OrangeJuice.Server.FSharp.Configuration

open System
open System.Collections.Generic
open System.Threading.Tasks

open OrangeJuice.Server.Cache
open OrangeJuice.Server.Configuration

type CachingAwsOptionsProvider(optionsProvider : IOptionsProvider<AwsOptions>, cacheClient : ICacheClient) =
    interface IOptionsProvider<AwsOptions> with
        member this.GetOptions() : Task<IEnumerable<AwsOptions>> =
            let valueFactory = new Func<Task<IEnumerable<AwsOptions>>>(fun() -> optionsProvider.GetOptions())
            cacheClient.AddOrGetExisting(AwsOptions.CacheKey, valueFactory)
