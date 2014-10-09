namespace OrangeJuice.Server.FSharp.Data.Logging

open Elmah
open Elmah.Contrib.EntityFramework

open OrangeJuice.Server.Configuration

type ErrorLogFactory(environmentProvider : IEnvironmentProvider, connectionStringProvider : IConnectionStringProvider) =
    interface Factory.IFactory<ErrorLog> with
        member this.Create() : ErrorLog =
            this.Create()

    member this.Create() =
        match environmentProvider.GetCurrentEnvironment() with
            | Environment.Production -> new EntityErrorLog(connectionStringProvider.GetDefaultConnectionString()) :> ErrorLog
            | _ -> new TraceErrorLog() :> ErrorLog
