namespace OrangeJuice.Server.FSharp.Configuration

open System
open OrangeJuice.Server.Configuration

type AwsOptionsFactory(configurationProvider : IConfigurationProvider) =
    interface Factory.IFactory<AwsOptions> with
        member this.Create() : AwsOptions =
            new AwsOptions(AccessKey = configurationProvider.GetValue("aws:AccessKey"),
                           AssociateTag = configurationProvider.GetValue("aws:AssociateTag"),
                           SecretKey = configurationProvider.GetValue("aws:SecretKey"),
                           RequestLimit = TimeSpan.Parse(configurationProvider.GetValue("aws:RequestLimit")))
