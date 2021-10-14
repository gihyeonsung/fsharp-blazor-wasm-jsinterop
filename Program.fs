namespace BlazorWasm

open Microsoft.AspNetCore.Components.Web;
open Microsoft.AspNetCore.Components.WebAssembly.Hosting;
open Microsoft.Extensions.DependencyInjection
open Microsoft.JSInterop
open System
open System.Net.Http

(*
Invoke an instance .NET method

To invoke an instance .NET method from JavaScript (JS):

    Pass the .NET instance by reference to JS by wrapping the instance in a DotNetObjectReference and calling Create on it.
    Invoke a .NET instance method from JS using invokeMethod or invokeMethodAsync from the passed DotNetObjectReference. The .NET instance can also be passed as an argument when invoking other .NET methods from JS.
    Dispose of the DotNetObjectReference.
*)

module Api =

  type Adder () =
    member val X: int = 0 with get, set
    member __.Add dx = __.X <- __.X + dx

  [<JSInvokable("createAdder")>]
  let createAdder () =
    task {
      let adder = Adder ()
      return DotNetObjectReference.Create<Adder>(adder)
    }

  [<JSInvokable("add")>]
  let add (adder: DotNetObjectReference<Adder>) dx =
    task {
      let adder = adder.Value
      do adder.Add dx
      return adder.X
    }

  [<JSInvokable("add")>]
  let destroyAdder (adder: DotNetObjectReference<Adder>) =
    task {
      do adder.Dispose ()
    }

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
