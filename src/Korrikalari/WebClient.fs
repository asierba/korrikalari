namespace Korrikalari

module WebClient =
    open System.Net.Http
    let get (url:string) = 
        use client = new HttpClient()
        let task = client.GetAsync(url)
        let result = task.Result
        result.Content.ReadAsStringAsync().Result