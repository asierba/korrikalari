open System
open Korrikalari

let getClosestStreet = GoogleMapsClient.getClosestStreet WebClient.get

let getMostRecentActivity = 
    StravaClient.getMostRecentActivity (WebClient.getWithHeaders [StravaClient.authHeader])

let getActivityCoordinates =
    StravaClient.getActivityCoordinates (WebClient.getWithHeaders [StravaClient.authHeader])


[<EntryPoint>]
let main argv = 
    // let streets = StreetRepository.getStreets |> Seq.sortBy(fun street -> street.name)
    // let printStreet (street:OsmConverter.Street) =
    //     printfn "%s" street.name
    //     street.points |> Seq.iter (fun point -> printfn "\t(%s, %s)" point.lat point.lon)
    // streets |> Seq.iter (printStreet)
    // streets  |> Seq.length |> printfn "%i streets in file!"
    
    let mostRecentActivity = getMostRecentActivity()
    // printfn "%i" mostRecentActivityId

    let coordinates = getActivityCoordinates mostRecentActivity
    // // coordinates |> Seq.iter (printfn "%A")
    
    let streetNames = coordinates |> Seq.map getClosestStreet|> Seq.choose id |> Seq.distinct
    streetNames |> Seq.iteri (fun i name -> printfn "%3i.- %s" (i+1) name)
    0 