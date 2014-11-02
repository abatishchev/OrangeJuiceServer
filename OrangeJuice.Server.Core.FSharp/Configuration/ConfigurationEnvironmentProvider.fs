namespace OrangeJuice.Server.FSharp.Configuration

open System
open OrangeJuice.Server.Configuration

type ConfigurationEnvironmentProvider(configurationProvider : IConfigurationProvider) =
    interface IEnvironmentProvider with
        member this.GetCurrentEnvironment() : string =
            let environment = configurationProvider.GetValue("environment:Name")
            if String.IsNullOrEmpty(environment) then environment
            else invalidOp "Current environment is null or empty"
 