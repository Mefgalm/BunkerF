namespace Bunker.Domain

module Task =
    open Helpers
    open Domain

    let create player challangeId taskName taskDescription taskAnswer =
        match player.OwnedCompanies
              |> List.collect (fun x -> x.Challanges)
              |> List.tryFind (fun x -> x.Id = challangeId) with
        | Some challange -> 
            Ok { Id = NewTask
                 Name = taskName
                 Challange = challange
                 Description = taskDescription
                 Answer = taskAnswer }
        | None -> Fail [ "Challange not found" ]
