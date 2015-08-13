namespace OrangeJuice.Server.FSharp.Configuration

open OrangeJuice.Server.Configuration

type AuthOptionsFactory(configurationProvider : IConfigurationProvider) = 
    interface Factory.IFactory<AuthOptions> with
        member this.Create() : AuthOptions = 
            new AuthOptions(Audience = configurationProvider.GetValue("auth0:Audience"),
                            CertificateKey = configurationProvider.GetValue("auth0:CertificateKey"), 
                            Issuer = configurationProvider.GetValue("auth0:Issuer"))
