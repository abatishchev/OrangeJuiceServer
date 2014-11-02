namespace OrangeJuice.Server.FSharp.Configuration

open OrangeJuice.Server.Configuration

type GoogleAuthOptionsFactory(configurationProvider : IConfigurationProvider) =
    interface Factory.IFactory<GoogleAuthOptions>
        with member this.Create() : GoogleAuthOptions =
            new GoogleAuthOptions(
                Audience = configurationProvider.GetValue("google:Audience"),
                CertificateKey = configurationProvider.GetValue("google:CertificateKey"),
                CertificateSecret = configurationProvider.GetValue("google:CertificateSecret"),
                ClientId = configurationProvider.GetValue("google:ClientId"),
                Issuer = configurationProvider.GetValue("google:Issuer"))
