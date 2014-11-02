namespace OrangeJuice.Server.FSharp.Configuration

open OrangeJuice.Server.Configuration

type WebConfigurationProvider() =
    interface IConfigurationProvider with
        member this.GetValue(key : string) : string =
            System.Web.Configuration.WebConfigurationManager.AppSettings.Item(key)
