namespace Korrikalari

open Microsoft.AspNetCore.Http
open Newtonsoft.Json.Linq

type Output = { activityId: int; coordinates: seq<Coordinate>; streets: seq<string>}

type MainMiddleware(next:RequestDelegate) =
    let getClosestStreet = GoogleMapsClient.getClosestStreet WebClient.get
    let getMostRecentActivity = 
        StravaClient.getMostRecentActivity (WebClient.getWithHeaders [StravaClient.authHeader])
    let getActivityCoordinates =
        StravaClient.getActivityCoordinates (WebClient.getWithHeaders [StravaClient.authHeader])

    
    member this.Invoke(context:HttpContext) =
        let mostRecentActivity = getMostRecentActivity()
        let coordinates = getActivityCoordinates mostRecentActivity |> Seq.cache
        let streetNames = coordinates |> Seq.map getClosestStreet|> Seq.choose id |> Seq.distinct

        let output = {
            activityId = mostRecentActivity.id;
            streets = streetNames;
            coordinates = coordinates            
        }
        let json = JObject.FromObject(output)
        context.Response.WriteAsync(json.ToString())  |> ignore
        next.Invoke(context)