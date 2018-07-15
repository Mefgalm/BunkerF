namespace Bunker.Domain

module Company =
    open Helpers
    open Domain

//    let findCompany companyId player =
//        match player.OwnedCompanies |> List.tryFind (fun x -> x.Id = companyId) with
//        | Some company -> Ok company
//        | None -> Fail [ "Company not found" ]
//    
//    let create companyName player =
//        { Id = CompanyId.NewCompany
//          Name = companyName
//          Owners = [ player ]
//          Challanges = []
//          JoinedPlayers = []
//          Teams = [] }
//    
//    let update player companyId companyName =
//        companyId |> (findCompany player >> bind (fun company -> Ok { company with Name = companyName }))
//    
//    let addOwner owner companyId player =
//        match owner |> findCompany companyId with
//        | Ok company -> 
//            if company.Owners |> List.exists ((=) player) then Fail [ "Player already added" ]
//            else Ok { company with Owners = player :: company.Owners }
//        | Fail errors -> Fail errors
