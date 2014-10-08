namespace OrangeJuice.Server.FSharp.Validation

open System.Xml
open System.Xml.Linq
open System.Xml.XPath

open OrangeJuice.Server

type XmlRequestValidator() =
    interface IValidator<XElement> with
        member this.IsValid(item : XElement) : bool =
            this.IsValid(item)

    member this.IsValid(item : XElement) =
        if item = null then false
        else
            let nm = new XmlNamespaceManager(new NameTable())
            nm.AddNamespace("x", item.Name.Namespace.ToString()) |> ignore
            let toBool e =
                match e with
                | null -> None
                | e -> Some(XElement.op_Explicit e : bool)

            let isValid = item.XPathSelectElement("x:Request/x:IsValid", nm) |> toBool
            isValid.Value
