open System
open Korrikalari

[<EntryPoint>]
let main argv = 
    let streets = StreetRepository.getStreets
    let coordinates = StravaClient.getActivityCoordinates 780826706

    // coordinates |> Seq.iter (printfn "%A")
    // streets |> Seq.iter (printfn "%A")
    // streets  |> Seq.length |> printfn "%i streets in file!"
    0 