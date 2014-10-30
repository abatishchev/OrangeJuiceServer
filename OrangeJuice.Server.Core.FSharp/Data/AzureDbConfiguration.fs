namespace OrangeJuice.Server.FSharp.Data.Configuration

open System
open System.Data.Entity
open System.Data.Entity.Infrastructure
open System.Data.Entity.SqlServer

type AzureDbConfiguration() =
    inherit DbConfiguration()
    do
        let f = fun () -> new SqlAzureExecutionStrategy() :> IDbExecutionStrategy
        base.SetExecutionStrategy("System.Data.SqlClient", new Func<IDbExecutionStrategy>(f))
