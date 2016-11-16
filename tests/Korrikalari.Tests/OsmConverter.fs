module OsmConverter

open System.Xml
open System.Xml.Linq

type Point = { lat: string; lon: string}
type Street = { name: string; points: Point list}

let xn name = XName.Get(name)


let convert xml:seq<Street> = 
    match xml with
    | "" -> Seq.empty
    | "<osm></osm>" -> Seq.empty
    | _ -> 

        let osmXmlElement = XElement.Parse(xml)

        let wayToStreet (wayXml:XElement)  =
            let tag = wayXml.Elements(xn "tag")|> Seq.find(fun tag -> tag.Attribute(xn "v") <> null)
            let streetName = tag.Attribute(xn "v").Value;

            let nds = wayXml.Elements(xn "nd")
            let ndrefs = nds |> Seq.map(fun nd ->  nd.Attribute(xn "ref").Value)
            let nodes = osmXmlElement.Elements(xn "node")
            let matchingNodes = ndrefs |> Seq.map(fun ndref -> nodes |> Seq.find(fun node -> node.Attribute(xn "id").Value = ndref))
            let points = matchingNodes |> Seq.map(fun node -> {lat= node.Attribute(xn "lat").Value; lon=node.Attribute(xn "lon").Value}) |> Seq.toList
            {name = streetName; points = points}

        osmXmlElement.Elements(xn "way") |> Seq.map wayToStreet