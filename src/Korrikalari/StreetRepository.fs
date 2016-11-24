namespace Korrikalari

module StreetRepository =

    open System.Xml.Linq

    let getStreets =
        let fileName = "OsmSamples/sample.osm"
        let osmXml = XElement.Load(fileName)
        OsmConverter.convert osmXml