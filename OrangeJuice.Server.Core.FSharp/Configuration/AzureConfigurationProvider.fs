﻿namespace OrangeJuice.Server.FSharp.Configuration

open OrangeJuice.Server.Configuration

type AzureConfigurationProvider() =
    interface IConfigurationProvider with
        member this.GetValue(key : string) : string =
            Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting(key)