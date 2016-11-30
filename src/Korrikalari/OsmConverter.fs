namespace Korrikalari

module OsmConverter =

    open System.Xml
    open System.Xml.Linq

    type Point = { lat: string; lon: string; id:string}
    type Street = { name: string; points: Point seq}

    let xn name = XName.Get(name)

    let toPoint (nodeXml:XElement) =
        { lat = nodeXml.Attribute(xn "lat").Value; lon = nodeXml.Attribute(xn "lon").Value; id = nodeXml.Attribute(xn "id").Value}

    let getStreetName (wayXml:XElement) =
        let nameTag = wayXml.Elements(xn "tag")|> Seq.find(fun tag -> tag.Attribute(xn "k").Value = "name")
        nameTag.Attribute(xn "v").Value;

    let ndToPoint nodes (ndXml:XElement) =
        let ndref = ndXml.Attribute(xn "ref").Value
        nodes |> Seq.find(fun node -> node.id = ndref)

    let getPoints nodes (wayXml:XElement) =
        let nds = wayXml.Elements(xn "nd")
        nds |> Seq.map (ndToPoint nodes) |> Seq.toList

    let wayToStreet nodes (wayXml:XElement)   =
            let streetName = getStreetName wayXml
            let points = getPoints nodes wayXml
            { name = streetName; points = points}
            
    let wayHasName (wayXml:XElement) =
            wayXml.Elements(xn "tag")|> Seq.exists(fun tag -> tag.Attribute(xn "k").Value = "name")

    let mergeDuplicates streets =
            streets |> 
            Seq.groupBy(fun street -> street.name) |> 
            Seq.map(fun (name, streetsByName) -> {name = name; points = streetsByName |> Seq.collect(fun street -> street.points) |> Seq.toList})

    let convert (osmXmlElement:XElement):seq<Street> = 
        let nodes = osmXmlElement.Elements(xn "node") |> Seq.map toPoint |> Seq.toList
        let ways = osmXmlElement.Elements(xn "way") |> Seq.filter wayHasName

        ways |> Seq.map (wayToStreet nodes) |> mergeDuplicates