namespace OrangeJuice.Server.FSharp.Configuration

open System.Reflection

open OrangeJuice.Server
open OrangeJuice.Server.Configuration

type AuthOptionsFactory(configurationProvider : IConfigurationProvider) =
    interface Factory.IFactory<AuthOptions> with
        member this.Create() : AuthOptions =
            this.Create()

    member this.Create() =
        new AuthOptions(
             Audience = configurationProvider.GetValue("auth0:Audience"),
             CertificateKey = configurationProvider.GetValue("auth0:CertificateKey"),
             Issuer = configurationProvider.GetValue("auth0:Issuer"))
