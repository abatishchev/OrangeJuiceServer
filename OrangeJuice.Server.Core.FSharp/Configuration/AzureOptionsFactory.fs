namespace OrangeJuice.Server.FSharp.Configuration

open OrangeJuice.Server.Configuration

type AzureOptionsFactory(configurationProvider : IConfigurationProvider) = 
    interface Factory.IFactory<AzureOptions> with
        member this.Create() : AzureOptions = 
            new AzureOptions(ConnectionString = configurationProvider.GetValue("azure:Blob"), 
                             ProductsContainer = configurationProvider.GetValue("azure:container:Products"), 
                             AwsOptionsContainer = configurationProvider.GetValue("azure:container:AwsOptions"))
