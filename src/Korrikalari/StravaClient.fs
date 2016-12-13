namespace Korrikalari

module StravaClient = 

    open System
    open System.IO
    open System.Net.Http
    open Newtonsoft.Json.Linq

    type Activity = { id:int }

    let getToken =
        System.IO.File.ReadLines("strava.token") |> Seq.head

    let getActivityCoordinates activity =
        let url = sprintf "https://www.strava.com/api/v3/activities/%i/streams/latlng" activity.id
        use client = new HttpClient()

        client.DefaultRequestHeaders.Add("Authorization", sprintf "Bearer %s" getToken)

        let task = client.GetAsync(url)
        let result = task.Result
        let content = result.Content.ReadAsStringAsync().Result
        let jarray = JArray.Parse(content)
        let first = (jarray |> Seq.toList).Head
        let data = first.Value<JArray>("data")
        let toCoordinate item =
            let coordinates = item |> Seq.map(float)
            { lat = Seq.item 0 coordinates; lon= Seq.item 1 coordinates}
        data |> Seq.map(toCoordinate)

    let getMostRecentActivity =
        let url = "https://www.strava.com/api/v3/activities?per_page=1" 
        use client = new HttpClient()

        client.DefaultRequestHeaders.Add("Authorization", sprintf "Bearer %s" getToken)

        let task = client.GetAsync(url)
        let result = task.Result
        let content = result.Content.ReadAsStringAsync().Result
        let jarray = JArray.Parse(content)
        let first = (jarray |> Seq.toList).Head
        { id = first.Value<int>("id") }