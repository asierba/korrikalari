namespace Korrikalari

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http

type Startup() = 

    member this.Configure(app: IApplicationBuilder) =
        app.UseDefaultFiles()
         .UseStaticFiles()
         .UseDeveloperExceptionPage()
         .UseMiddleware<MainMiddleware>()
         .Run(fun context -> context.Response.WriteAsync(""))
        