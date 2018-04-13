namespace Korrikalari

module StravaClient = 

    open System
    open System.IO
    open System.Net.Http
    open Newtonsoft.Json.Linq

    type Activity = { id:int }

    let getActivityCoordinates httpGet activity =
        let url = sprintf "https://www.strava.com/api/v3/activities/%i/streams/latlng" activity.id
        let content = httpGet url
        let jarray = JArray.Parse(content)
        let first = (jarray |> Seq.toList).Head
        let data = first.Value<JArray>("data")
        let toCoordinate item =
            let coordinates = item |> Seq.map(float)
            { lat = Seq.item 0 coordinates; lon= Seq.item 1 coordinates}
        data |> Seq.map(toCoordinate)

    let getMostRecentActivity httpGet () =
        let url = "https://www.strava.com/api/v3/activities?per_page=1" 
        let content = httpGet url
        let jarray = JArray.Parse(content)
        let first = (jarray |> Seq.toList).Head
        { id = first.Value<int>("id") }

    let authHeader = 
        let apiToken = System.Environment.GetEnvironmentVariable("STRAVA_TOKEN")
        ("Authorization", sprintf "Bearer %s" apiToken)
