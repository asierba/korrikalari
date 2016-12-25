open System
open Korrikalari
open Microsoft.AspNetCore.Hosting
open System.IO

[<EntryPoint>]
let main argv = 
    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseStartup<Startup>()
        .Build()
        .Run()
    0