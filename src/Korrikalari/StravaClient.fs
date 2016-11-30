namespace Korrikalari

module StravaClient = 

    open System
    open System.IO
    open System.Net.Http
    open Newtonsoft.Json.Linq

    type Coordinate = Coordinate of (float * float)

    let getToken =
        System.IO.File.ReadLines("strava.token") |> Seq.head

    let getActivityCoordinates activityId =
        let url = sprintf "https://www.strava.com/api/v3/activities/%i/streams/latlng" activityId
        use client = new HttpClient()

        client.DefaultRequestHeaders.Add("Authorization", sprintf "Bearer %s" getToken)

        let task = client.GetAsync(url)
        let result = task.Result
        let content = result.Content.ReadAsStringAsync().Result
        let jarray = JArray.Parse(content)
        let first = (jarray |> Seq.toList).Head
        let data = first.Value<JArray>("data")
        let toCoordinate item =
            let coordinates = item |> Seq.map(float) |> Seq.toList
            Coordinate (coordinates.[0], coordinates.[1])
        data |> Seq.map(toCoordinate)