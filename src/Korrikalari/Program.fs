open System
open System.Xml.Linq
open Korrikalari.OsmConverter

open Newtonsoft.Json

open System
open System.IO
open System.Net.Http
open Newtonsoft.Json.Linq
 
let getActivityCoordinates activityId =
    let url = sprintf "https://www.strava.com/api/v3/activities/%i/streams/latlng" activityId
    use client = new HttpClient()

    client.DefaultRequestHeaders.Add("Authorization", "Bearer X")

    let task = client.GetAsync(url)
    let result = task.Result
    let content = result.Content.ReadAsStringAsync().Result
    let jarray = JArray.Parse(content)
    let first = (jarray |> Seq.toList).Head
    let data = first.Value<JArray>("data")
    let toCoordinate item =
        let coordinates = item |> Seq.toList |> Seq.map(float) |> Seq.toList
        (coordinates.[0], coordinates.[1])
    data |> Seq.map(toCoordinate)

[<EntryPoint>]
let main argv = 
    let coordinates = getActivityCoordinates 780826706

    coordinates |> Seq.iter (printfn "%A")

   
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