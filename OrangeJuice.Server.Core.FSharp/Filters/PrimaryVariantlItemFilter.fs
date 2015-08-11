namespace OrangeJuice.Server.FSharp.Services

open System.Xml
open System.Xml.Linq
open System.Xml.XPath

open OrangeJuice.Server.Filters

type PrimaryVariantlItemFilter() =
    interface IFilter<XElement> with
        member this.Filter(element : XElement) : bool =
            let nm = new XmlNamespaceManager(new NameTable())
            nm.AddNamespace("x", element.Name.Namespace.ToString()) |> ignore

            Seq.isEmpty(element.XPathSelectElements("x:ImageSets/x:ImageSet[contains(@Category, 'variant')]", nm))
