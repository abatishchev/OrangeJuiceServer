namespace OrangeJuice.Server.FSharp.Configuration

open OrangeJuice.Server.Configuration

type AzureOptionsFactory(configurationProvider : IConfigurationProvider) = 
    interface Factory.IFactory<AzureOptions> with
        member this.Create() : AzureOptions = 
            new AzureOptions(ConnectionString = configurationProvider.GetValue("azure:ConnectionString"), 
                             ProductsContainer = configurationProvider.GetValue("azure:Products"), 
                             AwsOptionsContainer = configurationProvider.GetValue("azure:AwsOptions"))
