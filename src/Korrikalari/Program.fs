open System
open Korrikalari

open Microsoft.AspNetCore.Hosting

[<EntryPoint>]
let main argv = 
    let host = WebHostBuilder().UseKestrel().UseStartup<Startup>().Build()
    host.Run()
    0