namespace Korrikalari

module WebClient =
    open System.Net.Http
    
    let getWithHeaders (headers:seq<(string*string)>) (url:string) = 
        use client = new HttpClient()
        headers |> Seq.iter(fun header -> client.DefaultRequestHeaders.Add(fst header, snd header))
        let task = client.GetAsync(url)
        let result = task.Result
        result.Content.ReadAsStringAsync().Result
    
    let get (url:string) = 
        getWithHeaders Seq.empty url
