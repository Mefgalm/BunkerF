module BrainBunker.App

open Bunker.Database
open Bunker.Domain
open Giraffe
open Helpers
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open System
open System.IO
open System.Linq
open Bunker.Database.Repository

// ---------------------------------
// Models
// ---------------------------------
module DomainEntity =
    let mapPlayer (player : Domain.Player) =
        Entities.Player
            (NickName = PlayerNickName.value player.NickName, FirstName = PlayerFirstName.value player.FirstName, 
             LastName = PlayerLastName.value player.LastName, Email = Email.value player.Email, 
             PasswordHash = player.PasswordHash, PasswordSalt = player.PasswordSalt)

module EntityDomain =
    type ForwardBuilder() =
        
        member this.Bind(x, f) =
            match x with
            | Fail e -> raise <| Exception("Wrong mapper")
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

let result = new ResultBuilder()

// ---------------------------------
// Views
// ---------------------------------


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
      Email : string
      Password : string }

[<CLIMutable>]
type PlayerUpdateRequest =
    { Id : int
      NickName : string
      Name : string }

let userCreate model userExists createUser saveDb mapper =
    result { 
        let! isUserExists = userExists model.Email
        let! playerResult = createUser model.NickName model.FirstName model.LastName model.Email model.Password isUserExists
        return saveDb (playerResult |> mapper)
    }

let createUser : HttpHandler =
    fun next ctx -> 
        task { 
            let! model = ctx.BindJsonAsync<PlayerCreateRequest>()
            let dbContext = ctx.GetService<BunkerDbContext>()           
                        
            let createUserResult = userCreate model (PlayerRepository.emailExists dbContext) Player.create (Repository.save dbContext) DomainEntity.mapPlayer
                        
            return! json createUserResult next ctx
        }
  
let webApp =
    choose [ GET >=> choose [ route "/" >=> indexHandler "world"
                              routef "/hello/%s" indexHandler ]
             POST >=> choose [ route "/user" >=> createUser ]
             //             PUT >=> choose [
             //                route "user" >=> 
             //             ]
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
