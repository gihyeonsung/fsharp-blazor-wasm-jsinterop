namespace BlazorWasm

open Microsoft.AspNetCore.Components.Web;
open Microsoft.AspNetCore.Components.WebAssembly.Hosting;
open Microsoft.Extensions.DependencyInjection
open Microsoft.JSInterop
open System
open System.Net.Http

module Api =
  [<JSInvokable("add")>]
  let add x y = task { return x + y }

  [<JSInvokable("sub")>]
  let sub x y = task { return x - y }

module Program =
  [<EntryPoint>]
  let main args =
    let builder = WebAssemblyHostBuilder.CreateDefault(args)
    builder.Services.AddScoped<HttpClient>
      (fun _ ->
        new HttpClient (BaseAddress = Uri builder.HostEnvironment.BaseAddress))
    |> ignore
    builder.Build().RunAsync() |> ignore
    0
