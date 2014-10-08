namespace OrangeJuice.Server.FSharp.Services

open System.Security.Cryptography
open System.Text

open OrangeJuice.Server.Configuration

type AwsAlgorithmFactory(awsOptions : AwsOptions) =
    interface Factory.IFactory<HashAlgorithm> with
        member this.Create() : HashAlgorithm
            = this.Create()

    member this.Create() =
        let secret = Encoding.UTF8.GetBytes(awsOptions.SecretKey)
        new HMACSHA256(secret) :> HashAlgorithm