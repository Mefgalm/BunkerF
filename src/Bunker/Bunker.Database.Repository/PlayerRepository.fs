namespace Bunker.Database.Repository

open Bunker.Database
open Bunker.Domain
open Helpers
open System.Linq

module Repository =
    let save<'a when 'a : not struct> (dbContext : BunkerDbContext) entity =
        try 
            dbContext.Set<'a>().Add(entity) |> ignore
            dbContext.SaveChanges() |> ignore
            Ok()
        with e -> Fail [ e.Message ]
    
    let get<'a when 'a : not struct> (dbContext : BunkerDbContext) selector =
        match dbContext.Set<'a>() |> Seq.tryFind selector with
        | Some e -> Ok e
        | None -> Fail [ "Entity not found" ]
    
    let update<'a when 'a : not struct> (dbContext : BunkerDbContext) mapper domainObject =
        match domainObject with
        | Ok domObj -> 
            try 
                dbContext.Set<'a>().Update(domObj |> mapper) |> ignore
                dbContext.SaveChanges() |> ignore
                Ok domObj
            with e -> Fail [ e.Message ]
        | Fail e -> Fail e

module PlayerRepository =
    let emailExists (dbContext : BunkerDbContext) email =
        try 
            Ok <| dbContext.Players.Any(fun x -> x.Email = email)
        with e -> Fail [ e.Message ]
