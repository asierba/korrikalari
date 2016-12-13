namespace Korrikalari

module GoogleMapsClient = 
    open Newtonsoft.Json.Linq
    open System.Net.Http
    
    let getClosestStreet coordinate = 
        let url = sprintf "https://maps.googleapis.com/maps/api/geocode/json?latlng=%f,%f" coordinate.lat coordinate.lon
        use client = new HttpClient()

        let task = client.GetAsync(url)
        let result = task.Result
        let content = result.Content.ReadAsStringAsync().Result
        let jobject = JObject.Parse(content)
        let results = jobject.Value<JArray>("results")
        if (results.Count > 0) then
            let firstResult = (results |> Seq.toList).Head
            Some(firstResult.Value<string>("formatted_address"))
        else 
            None