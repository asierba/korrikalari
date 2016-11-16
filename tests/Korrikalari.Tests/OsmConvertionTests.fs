﻿module OsmConvertionTests

open Xunit
open OsmConverter

[<Theory>]
[<InlineData("")>]
[<InlineData("<osm></osm>")>]
let ``no content returns nothing`` osmXml =
    Assert.Equal<Street seq>(convert osmXml, Seq.empty)

[<Fact>]
let ``converts a way to a street``() =
    let osmXml = """
    <osm>
     <way>
      <tag v="Nowhere Street"/>
     </way>
    </osm>"""
    let streets = [{ name = "Nowhere Street"; points = List.empty }]

    let result = convert osmXml |> Seq.toList

    Assert.Equal<Street list>(result, streets)

[<Fact>]
let ``converts a way with points to a street``() =
    let osmXml = """
    <osm>
        <node id="292403538" lat="54.0901447" lon="12.2516513"/>
        <node id="298884289" lat="54.0901447" lon="13.2516513"/>
        <node id="261728686" lat="54.0901447" lon="14.2516513"/>
        <way>
            <nd ref="292403538"/>
            <nd ref="298884289"/>
            <nd ref="261728686"/>
            <tag v="Nowhere Street"/>
        </way>
    </osm>"""
    let points = [{ lat= "54.0901447"; lon= "12.2516513" }; { lat= "54.0901447"; lon= "13.2516513" }; { lat= "54.0901447"; lon= "14.2516513" }]
    let streets = [{ name = "Nowhere Street";points = points}]

    let result = convert osmXml |> Seq.toList

    Assert.Equal<Street list>(result, streets)

[<Fact>]
let ``converts multiple ways to streets``() =
    let osmXml = """<osm>
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
    </osm>"""
    let streets = [
    { name = "Street 1"; points = [{ lat= "54"; lon= "12" }; { lat= "54"; lon= "13" }; { lat= "54"; lon= "14" }]};
    { name = "Street 2"; points = [{ lat= "54"; lon= "14" }; { lat= "56"; lon= "23" }; { lat= "57"; lon= "43" }]}
    ]
 
    let result = convert osmXml |> Seq.toList

    Assert.Equal<Street list>(result, streets)