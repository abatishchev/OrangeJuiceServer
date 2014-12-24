namespace OrangeJuice.Server.FSharp.Services

open System
open System.Collections.Generic
open System.Xml
open System.Xml.Linq
open System.Xml.XPath

open OrangeJuice.Server
open OrangeJuice.Server.Services

type XmlItemSelector(itemValidator : IValidator<XElement>) =
    interface IItemSelector with
        member this.SelectItems(xml : string) : IEnumerable<XElement> =
            let doc = XDocument.Parse(xml)
            let ns = doc.Root.Name.Namespace

            let items = doc.Root.Element(ns + "Items")
            if itemValidator.IsValid(items) then items.Elements(ns + "Item")
            else raise <| new ArgumentException(XmlItemSelector.GetErrorMessage(doc, ns))

    static member GetErrorMessage(doc : XDocument, ns : XNamespace ) : string =
        let nm = new XmlNamespaceManager(new NameTable())
        nm.AddNamespace("x", ns.NamespaceName) |> ignore
        doc.XPathSelectElement("//x:Error", nm).Value
