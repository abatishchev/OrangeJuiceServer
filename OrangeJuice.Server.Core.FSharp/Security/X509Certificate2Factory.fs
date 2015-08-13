namespace OrangeJuice.Server.FSharp.Security

open System
open System.Security.Cryptography
open System.Security.Cryptography.X509Certificates

open OrangeJuice.Server.Configuration
open OrangeJuice.Server.Security

type X509Certificate2Factory(authOptions : GoogleAuthOptions, environmentProvider : IEnvironmentProvider) =
    interface Factory.IFactory<X509Certificate2> with
        member this.Create() : X509Certificate2 =
            match environmentProvider.GetCurrentEnvironment() with
            | EnvironmentName.Production -> X509Certificate2Factory.Create(authOptions.CertificateThumbprint)
            | _ -> X509Certificate2Factory.Create(authOptions.CertificateKey, authOptions.CertificateSecret)

    static member Create(key : string, secret : string) : X509Certificate2 =
        new X509Certificate2(Convert.FromBase64String(key), secret)

    static member Create(thumbprint : string) : X509Certificate2 =
        let certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser)
        certStore.Open(OpenFlags.ReadOnly) |> ignore

        let certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false)
        let cert = certCollection
                   |> Seq.cast<X509Certificate2>
                   |> Seq.head

        certStore.Close() |> ignore

        if cert <> null then cert
        else raise <| new CryptographicException(sprintf "Certificate with thumbprint '%s' was not found in store '%s'" thumbprint certStore.Name)
