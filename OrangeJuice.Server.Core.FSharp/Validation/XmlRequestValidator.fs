namespace OrangeJuice.Server.FSharp.Validation

open System.Xml
open System.Xml.Linq
open System.Xml.XPath

open OrangeJuice.Server

type XmlRequestValidator() =
    interface IValidator<XElement> with
        member this.IsValid(item : XElement) : bool =
            if item = null
            then false
            else
                let nm = new XmlNamespaceManager(new NameTable())
                nm.AddNamespace("x", item.Name.Namespace.ToString()) |> ignore

                let toBool e =
                    let o = if e <> null
                                then Some(XElement.op_Explicit e : bool)
                                else None
                    if o.IsSome
                        then o.Value
                        else false

                item.XPathSelectElement("x:Request/x:IsValid", nm) |> toBool
