﻿namespace OrangeJuice.Server.FSharp.Data

open System.Reflection

open OrangeJuice.Server
open OrangeJuice.Server.Configuration
open OrangeJuice.Server.Data

type ApiVersionFactory(assemblyProvider : IAssemblyProvider, environmentProvider : IEnvironmentProvider) =
    interface Factory.IFactory<ApiVersion> with
        member this.Create() : ApiVersion =
            this.Create()

    member this.Create() =
        let version = assemblyProvider.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version
        let environment = environmentProvider.GetCurrentEnvironment()
        new ApiVersion(Version = version, Environment = environment)