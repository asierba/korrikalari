namespace Korrikalari

module GoogleMapsClient = 
    open Newtonsoft.Json.Linq
    open System.IO

    type StreetName = string Option

    let getApiKey =
        File.ReadLines("gmaps.key") |> Seq.head

    let hasRouteType (element:JToken) =
                element.Value<JArray>("types") |> Seq.exists(fun i -> i.Value<string>() = "route")

    let getClosestStreet httpGet coordinate : StreetName = 
        let url = sprintf "https://maps.googleapis.com/maps/api/geocode/json?latlng=%f,%f&result_type=street_address&key=%s" coordinate.lat coordinate.lon getApiKey
        let content = httpGet url
        let contentJson = JObject.Parse(content)
        let results = contentJson.Value<JArray>("results")
        match (results.Count) with
        | 0 -> None
        | _ -> 
            let closestAddress = results.[0]
            let street = closestAddress.Value<JArray>("address_components") |> Seq.find(hasRouteType)
            let streetName = street.Value<string>("long_name")
            Some streetName