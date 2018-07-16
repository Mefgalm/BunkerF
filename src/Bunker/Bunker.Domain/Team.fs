namespace Bunker.Domain

module Team =
    open Domain
    open Helpers
    
    
    
//    let findTeam player teamId =
//        match player.OwnedTeams |> List.tryFind (fun x -> x.Id = teamId) with
//        | Some company -> Ok company
//        | None -> Fail [ "Team not found" ]
//    
//    let create player company teamName teamDescription =
//        match player.OwnedTeams |> List.exists (fun x -> x.Company = company) with
//        | true -> Fail [ "Player can have only one team in company" ]
//        | false -> 
//            Ok { Id = NewTeam
//                 Name = teamName
//                 Owner = player
//                 Description = teamDescription
//                 Company = company
//                 Players = []
//                 Challanges = [] }
//    
//    let update player teamId teamName teamDescription =
//        match player |> findTeam teamId with
//        | Ok team -> 
//            Ok { team with Name = teamName
//                           Description = teamDescription }
//        | Fail errors -> Fail errors
