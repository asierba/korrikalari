namespace Korrikalari

module OsmConverter =

    open System.Xml
    open System.Xml.Linq

    type Point = { lat: string; lon: string; id:string}
    type Street = { name: string; points: Point list}

    let xn name = XName.Get(name)

    let toPoint (nodeXml:XElement) =
        { lat= nodeXml.Attribute(xn "lat").Value; lon=nodeXml.Attribute(xn "lon").Value;id=nodeXml.Attribute(xn "id").Value}

    let convert (osmXmlElement:XElement):seq<Street> = 
        let nodes = osmXmlElement.Elements(xn "node") |> Seq.map toPoint |> Seq.toList

        let wayToStreet (wayXml:XElement)  =
            let nameTag = wayXml.Elements(xn "tag")|> Seq.find(fun tag -> tag.Attribute(xn "k").Value = "name")
            let streetName = nameTag.Attribute(xn "v").Value;

            let nds = wayXml.Elements(xn "nd")
            let ndrefs = nds |> Seq.map(fun nd ->  nd.Attribute(xn "ref").Value)

            let points = ndrefs |> Seq.map(fun ndref -> nodes |> Seq.find(fun node -> node.id = ndref)) |> Seq.toList

            {name = streetName; points = points}

        let wayHasName (wayXml:XElement) =
            wayXml.Elements(xn "tag")|> Seq.exists(fun tag -> tag.Attribute(xn "k").Value = "name")

        osmXmlElement.Elements(xn "way") |> Seq.filter wayHasName |> Seq.map wayToStreet