namespace Korrikalari

module StravaClient = 

    open Newtonsoft.Json.Linq

    type Activity = { id:int }

    let getActivityCoordinates httpGet activity =
        let toCoordinate item =
            let coordinates = item |> Seq.map(float)
            { lat = Seq.item 0 coordinates; lon= Seq.item 1 coordinates}

        let url = sprintf "https://www.strava.com/api/v3/activities/%i/streams/latlng" activity.id
        let content = httpGet url
        let jarray = JArray.Parse(content)
        let latlngs = (jarray |> Seq.toList) |> List.filter(fun x -> x.Value<string>("type") = "latlng")
        if List.isEmpty latlngs then
            Seq.empty
        else
            let latlong = latlngs |> List.head
            latlong.Value<JArray>("data") |> Seq.map(toCoordinate)

    let getMostRecentActivity httpGet () =
        let url = "https://www.strava.com/api/v3/activities?per_page=1" 
        let content = httpGet url
        let jarray = JArray.Parse(content)
        let first = (jarray |> Seq.toList).Head
        { id = first.Value<int>("id") }

    let authHeader = 
        let apiToken = System.Environment.GetEnvironmentVariable("STRAVA_TOKEN")
        ("Authorization", sprintf "Bearer %s" apiToken)
