namespace Korrikalari.Tests

module OsmConvertionTests =

    open Xunit
    open Korrikalari.OsmConverter
    open System.Xml.Linq

    let createXml xml =
        XElement.Parse(xml)

    [<Fact>]
    let ``no content returns no streets`` =
        let osmXml = createXml "<osm></osm>"
        Assert.Empty(convert osmXml)

    [<Fact>]
    let ``converts a way to a street``() =
        let osmXml = createXml """
        <osm>
            <way>
                <tag k="highway" v="residential"/>
                <tag k="name" v="Nowhere Street"/>
            </way>
        </osm>"""
        let streets = [{ name = "Nowhere Street"; points = List.empty }]

        let result = convert osmXml |> Seq.toList

        Assert.Equal<Street list>(result, streets)

    [<Fact>]
    let ``ignore ways with no name``() =
        let osmXml = createXml """
        <osm>
            <way>
                <tag k="highway" v="residential"/>
            </way>
        </osm>"""
        let streets = []

        Assert.Empty(convert osmXml)

    [<Fact>]
    let ``converts a way with points to a street``() =
        let osmXml = createXml """
        <osm>
            <node id="292403538" lat="54.0901447" lon="12.2516513"/>
            <node id="298884289" lat="54.0901447" lon="13.2516513"/>
            <node id="261728686" lat="54.0901447" lon="14.2516513"/>
            <way>
                <nd ref="292403538"/>
                <nd ref="298884289"/>
                <nd ref="261728686"/>
                <tag k="name" v="Nowhere Street"/>
            </way>
        </osm>"""

        let result = convert osmXml |> Seq.toList

        let streets = [{ name = "Nowhere Street"; 
                        points = [{ lat= "54.0901447"; lon= "12.2516513"; id="292403538" }; 
                                  { lat= "54.0901447"; lon= "13.2516513"; id="298884289" }; 
                                  { lat= "54.0901447"; lon= "14.2516513"; id="261728686" }]
                       }]
        Assert.Equal<Street list>(result, streets)

    [<Fact>]
    let ``converts multiple ways to streets``() =
        let osmXml = XElement.Parse("""<osm>
        <node id="1" lat="54" lon="12"/>
        <node id="2" lat="54" lon="13"/>
        <node id="3" lat="54" lon="14"/>
        <node id="4" lat="56" lon="23"/>
        <node id="5" lat="57" lon="43"/>
        <way>
            <nd ref="1"/>
            <nd ref="2"/>
            <nd ref="3"/>
            <tag k="highway" />
            <tag k="name" v="Street 1"/>
        </way>
        <way>
            <nd ref="3"/>
            <nd ref="4"/>
            <nd ref="5"/>
            <tag k="highway" />
            <tag k="name" v="Street 2"/>
        </way>
        </osm>""")
        
    
        let result = convert osmXml |> Seq.toList

        let streets = [ 
            {   name = "Street 1"; 
                points = [  { lat= "54"; lon= "12"; id="1" };
                            { lat= "54"; lon= "13"; id="2" }; 
                            { lat= "54"; lon= "14"; id="3" }]
            }; 
            {   name = "Street 2"; 
                points = [  { lat= "54"; lon= "14"; id="3" }; 
                            { lat= "56"; lon= "23"; id="4" }; 
                            { lat= "57"; lon= "43"; id="5" }]}
            ]
        Assert.Equal<Street list>(result, streets)