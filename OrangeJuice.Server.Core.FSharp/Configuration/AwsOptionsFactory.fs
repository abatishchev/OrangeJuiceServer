namespace OrangeJuice.Server.FSharp.Configuration

open System

open OrangeJuice.Server.Configuration

type AwsOptionsFactory() =
    [<Literal>] 
    let AwsAccess = "AKIAICFWNOWCE42LO7BQ"
    [<Literal>] 
    let AwsAssociate = "orang04-20"
    [<Literal>] 
    let AwsSecret = "zcSSMQbyvjchQHmtA4nNftsGNxNwBOgfUZr1ok1+"
 
    interface Factory.IFactory<AwsOptions> with
        member this.Create() : AwsOptions =
            this.Create()

    member this.Create() =
        new AwsOptions(
            AccessKey = AwsAccess,
            AssociateTag = AwsAssociate,
            SecretKey = AwsSecret,
            RequestLimit = TimeSpan.FromMilliseconds(1000.0))
