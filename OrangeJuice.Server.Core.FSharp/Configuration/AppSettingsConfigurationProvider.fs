namespace OrangeJuice.Server.FSharp.Configuration

open OrangeJuice.Server.Configuration

type AppSettingsConfigurationProvider() =
    interface IConfigurationProvider with
        member this.GetValue(key : string) : string =
            System.Configuration.ConfigurationManager.AppSettings.[key]
