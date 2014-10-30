namespace OrangeJuice.Server.FSharp.Configuration

open OrangeJuice.Server.Configuration

type AzureOptionsFactory(configurationProvider : IConfigurationProvider) =
    interface Factory.IFactory<AzureOptions> with
        member this.Create() : AzureOptions =
            this.Create()

    member this.Create() =
        new AzureOptions(
            ConnectionString = configurationProvider.GetValue("blob:ConnectionString"),
            ProductsContainer = configurationProvider.GetValue("blob:Products"))
