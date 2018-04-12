namespace Korrikalari
open System.IO

module FileSystemCache = 
    let replaceSpecialChars (filename:string) =
        let invalidChars = Path.GetInvalidFileNameChars()
        invalidChars |> Array.fold(fun (acc:string) char -> acc.Replace(char.ToString(), "_")) filename
        
    let cache httpGet url =
        let fileName = Path.Combine("cache", (replaceSpecialChars url))
        
        if not (Directory.Exists("cache")) then
            Directory.CreateDirectory("cache") |> ignore

        match File.Exists(fileName) with
        | true -> File.ReadAllText(fileName)
        | false -> 
            let result = httpGet url
            File.WriteAllText(fileName, result)
            result