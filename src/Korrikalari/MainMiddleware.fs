namespace Korrikalari

open Microsoft.AspNetCore.Http
open Newtonsoft.Json.Linq

type Output = { activityId: int; streets: seq<string>; coordinates: seq<Coordinate>}

type MainMiddleware(next:RequestDelegate) =
    let getClosestStreet = GoogleMapsClient.getClosestStreet WebClient.get
    let getMostRecentActivity = 
        StravaClient.getMostRecentActivity (WebClient.getWithHeaders [StravaClient.authHeader])
    let getActivityCoordinates =
        StravaClient.getActivityCoordinates (WebClient.getWithHeaders [StravaClient.authHeader])

    
    member this.Invoke(context:HttpContext) =
        if (context.Request.Method = "GET" && (context.Request.Path.ToString() = "/api/activity/last")) then
            let mostRecentActivity = getMostRecentActivity()
            let coordinates = getActivityCoordinates mostRecentActivity |> Seq.cache
            let streetNames = coordinates |> Seq.map getClosestStreet|> Seq.choose id |> Seq.distinct
            // let streetNames = [ "street 1"; "street 2"] |> List.toSeq

            let output = {
                activityId = mostRecentActivity.id;
                streets = streetNames;
                coordinates = coordinates            
            }
            let json = JObject.FromObject(output)
            context.Response.WriteAsync(json.ToString())  |> ignore

        next.Invoke(context)