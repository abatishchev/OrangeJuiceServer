namespace OrangeJuice.Server.FSharp.Configuration

open OrangeJuice.Server.Configuration

type ConfigurationConnectionStringProvider(configurationProvider : IConfigurationProvider) =
    interface IConnectionStringProvider with
        member this.GetDefaultConnectionString() : string =
            configurationProvider.GetValue("sql:ConnectionString")