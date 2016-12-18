namespace Korrikalari

module GoogleMapsClient = 
    open Newtonsoft.Json.Linq
    let getClosestStreet getFunc coordinate = 
        let url = sprintf "https://maps.googleapis.com/maps/api/geocode/json?latlng=%f,%f" coordinate.lat coordinate.lon
        let content = getFunc(url)
        let jobject = JObject.Parse(content)
        let results = jobject.Value<JArray>("results")
        if (results.Count > 0) then
            let firstResult = (results |> Seq.toList).Head
            Some(firstResult.Value<string>("formatted_address"))
        else 
            None