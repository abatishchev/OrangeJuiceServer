namespace OrangeJuice.Server.FSharp.Services

open OrangeJuice.Server.Services

type JsonBlobNameResolver() =
    interface IBlobNameResolver with
        member this.Resolve(blobName : string) : string = this.Resolve blobName
    
    member this.Resolve(blobName : string) = sprintf "%s.json" blobName