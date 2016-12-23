namespace Korrikalari

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http

type Startup() = 

    member this.Configure(app: IApplicationBuilder) =
        app.UseMiddleware<MainMiddleware>() |> ignore

        app.Run(fun context -> context.Response.WriteAsync(""))