namespace Korrikalari

module GoogleMapsClient = 
    open Newtonsoft.Json.Linq

    let getApiKey =
        System.IO.File.ReadLines("gmaps.key") |> Seq.head

    let getClosestStreet getFunc coordinate = 
        let url = sprintf "https://maps.googleapis.com/maps/api/geocode/json?latlng=%f,%f&result_type=street_address&key=%s" coordinate.lat coordinate.lon getApiKey
        let content = getFunc(url)
        let jobject = JObject.Parse(content)
        let results = jobject.Value<JArray>("results")
        if (results.Count > 0) then
            let firstResult = (results |> Seq.toList).Head

            let hasRouteType (element:JToken) =
                element.Value<JArray>("types") |> Seq.exists(fun i -> i.Value<string>() = "route")

            let street = firstResult.Value<JArray>("address_components") |> Seq.find(hasRouteType)
            let streetName = street.Value<string>("long_name")
            Some(streetName)
        else 
            None