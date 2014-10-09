namespace OrangeJuice.Server.FSharp.Data

open System.Xml
open System.Xml.Linq
open System.Xml.XPath

open OrangeJuice.Server.Data

type XmlProductDescriptorFactory() =
    interface Factory.IFactory<ProductDescriptor, XElement> with
        member this.Create(element : XElement) : ProductDescriptor =
            this.Create(element)

    member this.Create(element : XElement) =
        let nm = new XmlNamespaceManager(new NameTable())
        nm.AddNamespace("x", element.Name.Namespace.ToString()) |> ignore

        let toString e =
            match e with
                | null -> None
                | e -> Some(XElement.op_Explicit e : string)

        new ProductDescriptor(
            SourceProductId = (element.XPathSelectElement("x:ASIN", nm) |> toString).Value,
            Title = (element.XPathSelectElement("x:ItemAttributes/x:Title", nm) |> toString).Value,
            Brand = (element.XPathSelectElement("x:ItemAttributes/x:Brand", nm) |> toString).Value,
            SmallImageUrl =  (element.XPathSelectElement("x:SmallImage/x:URL", nm) |> toString).Value,
            MediumImageUrl = (element.XPathSelectElement("x:MediumImage/x:URL", nm) |> toString).Value,
            LargeImageUrl = (element.XPathSelectElement("x:LargeImage/x:URL", nm) |> toString).Value)
