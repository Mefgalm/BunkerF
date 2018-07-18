namespace Bunker.Domain

module Answer =
    open Helpers
    open Domain

//    let create (player : Player) task answer =
//        match player.JoinedTeams
//              |> List.collect (fun x -> x.Challanges)
//              |> List.collect (fun x -> x.Tasks)
//              |> List.tryFind ((=) task) with
//        | Some task -> 
//            if task.Answer = answer then 
//                Ok { Id = NewAnswer
//                     Task = task
//                     Player = player }
//            else Fail [ "Answer is wrong" ]
//        | None -> Fail [ "Task not found" ]
