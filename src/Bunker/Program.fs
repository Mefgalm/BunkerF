module BrainBunker.App

open Bunker.Database
open Domain
open Giraffe
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open System
open System.IO

// ---------------------------------
// Models
// ---------------------------------
module DomainEntity =
    let mapPlayer player =
        Bunker.Database.Entities.Player(
            NickName = PlayerNickName.value player.NickName,
            FirstName = PlayerFirstName.value player.FirstName,
            LastName = "",
            Email = "mefgalm@gmail.com",
            PasswordHash = [|(byte) 1|],
            PasswordSalt = [|(byte) 3|]            
        )               

type Message =
    { Text : string }

// ---------------------------------
// Views
// ---------------------------------

module Repository =
    
    let saveToDb<'a when 'a : not struct> (dbContext: BunkerDbContext) mapper domainObject =                 
        match domainObject with
        | Ok domObj -> 
            try 
                dbContext.Set<'a>().Add(domObj |> mapper) |> ignore
                dbContext.SaveChanges() |> ignore
                Ok domObj
            with e -> Fail [e.Message] 
        | Fail e -> Fail e                

module Views =
    open GiraffeViewEngine
    
    let layout (content : XmlNode list) =
        html [] [ head [] [ title [] [ encodedText "BrainBunker" ]
                            link [ _rel "stylesheet"
                                   _type "text/css"
                                   _href "/main.css" ] ]
                  body [] content ]
    
    let partial() = h1 [] [ encodedText "BrainBunker" ]
    
    let index (model : Message) =
        [ partial()
          p [] [ encodedText model.Text ] ]
        |> layout

// ---------------------------------
// Web app
// ---------------------------------
let indexHandler (name : string) =
    let greetings = sprintf "Hello %s, from Giraffe!" name
    let model = { Text = greetings }
    let view = Views.index model
    htmlView view

[<CLIMutable>]
type PlayerCreateRequest =
    { NickName : string
      Name : string }

let createUser : HttpHandler =
    fun next ctx -> 
        task { 
            let! model = ctx.BindJsonAsync<PlayerCreateRequest>()            
            let playerResult = Player.create model.NickName model.Name            
            let dbContext = ctx.GetService<BunkerDbContext>()            
            let saveToDb = Repository.saveToDb dbContext DomainEntity.mapPlayer playerResult
            
            return! json saveToDb next ctx
        }

let webApp =
    choose [ GET >=> choose [ route "/" >=> indexHandler "world"
                              routef "/hello/%s" indexHandler ]
             POST >=> choose [ route "/user" >=> createUser ]
             setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------
let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------
let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader() |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IHostingEnvironment>()
    (match env.IsDevelopment() with
     | true -> app.UseDeveloperExceptionPage()
     | false -> app.UseGiraffeErrorHandler errorHandler).UseCors(configureCors).UseStaticFiles().UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddEntityFrameworkNpgsql().AddDbContext<BunkerDbContext> 
        (fun o -> o.UseNpgsql("User ID=postgres;Password=1Q2w3e4r;Host=localhost;Database=Bunker;") |> ignore) |> ignore
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    let filter (l : LogLevel) = l.Equals LogLevel.Error
    builder.AddFilter(filter).AddConsole().AddDebug() |> ignore

let CreateWebHostBuilder args =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot = Path.Combine(contentRoot, "WebRoot")
    WebHost.CreateDefaultBuilder(args).UseKestrel().UseContentRoot(contentRoot).UseIISIntegration().UseWebRoot(webRoot)
           .Configure(Action<IApplicationBuilder> configureApp).ConfigureServices(configureServices)
           .ConfigureLogging(configureLogging)

[<EntryPoint>]
let main args =
    CreateWebHostBuilder(args).Build().Run()
    0
