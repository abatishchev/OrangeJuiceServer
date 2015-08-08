namespace OrangeJuice.Server.FSharp.Data

open System
open System.Xml
open System.Xml.Linq
open System.Xml.XPath
open OrangeJuice.Server.Data.Models

type XmlProductDescriptorFactory() = 
    
    let toString e = 
        let o = 
            if e <> null then Some(XElement.op_Explicit e : string)
            else None
        if o.IsSome then o.Value
        else null
    
    let toFloat e = 
        let o = 
            if e <> null then Some(XElement.op_Explicit e : float32)
            else None
        if o.IsSome then o.Value
        else 0.0f
    
    interface Factory.IFactory<ProductDescriptor, XElement> with
        member this.Create(element : XElement) : ProductDescriptor = this.Create(element)
    
    member this.Create(element : XElement) = 
        let nm = new XmlNamespaceManager(new NameTable())
        nm.AddNamespace("x", element.Name.Namespace.ToString()) |> ignore
        new ProductDescriptor(SourceProductId = (element.XPathSelectElement("x:ASIN", nm) |> toString), 
                              DetailsPageUrl = new Uri(element.XPathSelectElement("x:DetailPageURL", nm) |> toString), 
                              
                              Title = (element.XPathSelectElement("x:ItemAttributes/x:Title", nm) |> toString), 
                              Brand = (element.XPathSelectElement("x:ItemAttributes/x:Brand", nm) |> toString), 
                              
                              SmallImageUrl = (element.XPathSelectElement("x:SmallImage/x:URL", nm) |> toString), 
                              MediumImageUrl = (element.XPathSelectElement("x:MediumImage/x:URL", nm) |> toString), 
                              LargeImageUrl = (element.XPathSelectElement("x:LargeImage/x:URL", nm) |> toString), 
                              
                              LowestNewPrice = (element.XPathSelectElement("x:OfferSummary/x:LowestNewPrice/x:Amount", nm) |> toFloat),
                              
                              CustomerReviewsUrl = new Uri(element.XPathSelectElement("x:ItemLinks/x:ItemLink[x:Description/. = 'All Customer Reviews']/x:URL", nm) |> toString ))
