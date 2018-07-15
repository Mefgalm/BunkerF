module BrainBunker.App

open Bunker.Database
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
open Bunker.Database
open Bunker.Domain
open Helpers

// ---------------------------------
// Models
// ---------------------------------
module DomainEntity =
    let mapPlayer (player: Domain.Player) =
        Entities.Player(
            NickName = PlayerNickName.value player.NickName,
            FirstName = PlayerFirstName.value player.FirstName,
            LastName = PlayerLastName.value player.LastName,
            Email = Email.value player.Email,
            PasswordHash = player.PasswordHash,
            PasswordSalt = player.PasswordSalt                       
        )  
        
module EntityDomain =
    
    type ForwardBuilder() =
    
        member this.Bind(x, f) =
            match x with
            | Fail e -> raise <| Exception("")
            | Ok a -> f a
        
        member this.Return(x) = x

    let private forward = new ForwardBuilder()
//
//    let mapPlayer (player: Entities.Player) = forward {
//            let! nickName = PlayerNickName.create player.NickName
//            let! firstName = PlayerFirstName.create player.FirstName
//            
//            return  { Id = PlayerId.Id player.Id
//                      NickName = nickName
//                      FirstName = firstName
//                      JoinedCompanies = []
//                      JoinedTeams = []
//                      OwnedCompanies = []
//                      OwnedTeams = [] }        
//        }

type Message =
    { Text : string }
    

type MaybeBuilder() =
    
    member this.Bind(x, f) =
        match x with
        | None -> None
        | Some e -> e
    
    member this.Return(x) = Some x
   
let maybe = new MaybeBuilder() 

// ---------------------------------
// Views
// ---------------------------------

module Repository =
    
    let save<'a when 'a : not struct> (dbContext: BunkerDbContext) mapper domainObject =                 
        match domainObject with
        | Ok domObj -> 
            try 
                dbContext.Set<'a>().Add(domObj |> mapper) |> ignore
                dbContext.SaveChanges() |> ignore
                Ok domObj
            with e -> Fail [e.Message] 
        | Fail e -> Fail e
        
    let get<'a when 'a : not struct> (dbContext: BunkerDbContext) selector =
        match dbContext.Set<'a>() |> Seq.tryFind selector with
        | Some e -> Ok e
        | None -> Fail ["Entity not found"]
        
    let update<'a when 'a : not struct> (dbContext: BunkerDbContext) mapper domainObject =                 
        match domainObject with
        | Ok domObj -> 
            try 
                dbContext.Set<'a>().Update(domObj |> mapper) |> ignore
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
      FirstName : string
      LastName : string
      Email: string
      Password: string }
      
[<CLIMutable>]
type PlayerUpdateRequest =
    { Id : int
      NickName : string
      Name : string }

let createUser : HttpHandler =
    fun next ctx -> 
        task { 
            let! model = ctx.BindJsonAsync<PlayerCreateRequest>()
            let dbContext = ctx.GetService<BunkerDbContext>()
            let! isUserExists = dbContext.Players.AnyAsync(fun x -> x.Email = model.Email)                 
            let playerResult = Player.create model.NickName model.FirstName model.LastName model.Email model.Password isUserExists                     
            let saveToDb = Repository.save dbContext DomainEntity.mapPlayer playerResult
            
            return! json saveToDb next ctx
        }
//
//let updateUser : HttpHandler =
//    fun next ctx ->
//        task {
//            let! model = ctx.BindJsonAsync<PlayerUpdateRequest>()
//            let dbContext = ctx.GetService<BunkerDbContext>()
//            let dbUpdate = result {
//                let! dbPlayer = Repository.get<Entities.Player> dbContext (fun x -> x.Id = model.Id)
//            
//                let mapPlayer = EntityDomain.mapPlayer dbPlayer
//                let player = Player.update mapPlayer model.NickName model.Name
//                
//                return Repository.update dbContext DomainEntity.mapPlayer player
//            }
//            return! json dbUpdate next ctx
//        }        

let webApp =
    choose [ GET >=> choose [ route "/" >=> indexHandler "world"
                              routef "/hello/%s" indexHandler ]
             POST >=> choose [ route "/user" >=> createUser ]
             //PUT >=> choose [ route "/user" >=> updateUser              
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
