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
            let o = if e <> null
                        then Some(XElement.op_Explicit e : string)
                        else None
            if o.IsSome
                then o.Value
                else null

        new ProductDescriptor(
            SourceProductId = (element.XPathSelectElement("x:ASIN", nm) |> toString),
            Title = (element.XPathSelectElement("x:ItemAttributes/x:Title", nm) |> toString),
            Brand = (element.XPathSelectElement("x:ItemAttributes/x:Brand", nm) |> toString),
            SmallImageUrl =  (element.XPathSelectElement("x:SmallImage/x:URL", nm) |> toString),
            MediumImageUrl = (element.XPathSelectElement("x:MediumImage/x:URL", nm) |> toString),
            LargeImageUrl = (element.XPathSelectElement("x:LargeImage/x:URL", nm) |> toString))
