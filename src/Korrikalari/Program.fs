open System
open System.Xml.Linq
open Korrikalari.OsmConverter

[<EntryPoint>]
let main argv = 
    let fileName = "OsmSamples/test.osm"

    printfn "Loading OSM File..."
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let osmXml = XElement.Load(fileName)
    stopWatch.Stop()
    printfn "Time: %f s" stopWatch.Elapsed.TotalSeconds 
    
    printfn "Mapping XML Data to streets..."    
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let streets = convert osmXml |> Seq.toList
    stopWatch.Stop()
    printfn "Time: %f s" stopWatch.Elapsed.TotalSeconds

    let printStreetName street =
        printfn "%s" street.name

    printfn "Streets in OSM File:"
    streets |> Seq.iter printStreetName

    streets  |> List.length |> printfn "%i streets in file!"

    0 