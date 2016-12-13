open System
open Korrikalari

let measure action =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let result = action()
    stopWatch.Stop()
    printfn "%fs: %A" stopWatch.Elapsed.TotalSeconds action
    result

[<EntryPoint>]
let main argv = 
    // let streets = StreetRepository.getStreets |> Seq.sortBy(fun street -> street.name)
    // let printStreet (street:OsmConverter.Street) =
    //     printfn "%s" street.name
    //     street.points |> Seq.iter (fun point -> printfn "\t(%s, %s)" point.lat point.lon)
    // streets |> Seq.iter (printStreet)
    // streets  |> Seq.length |> printfn "%i streets in file!"

    let coordinates = StravaClient.getActivityCoordinates 780826706
    // coordinates |> Seq.iter (printfn "%A")

    let streetNames = coordinates |> Seq.map  GoogleMapsClient.getClosestStreet |> Seq.choose id |> Seq.distinct
    streetNames |> Seq.iter (printfn "%A")
    0 