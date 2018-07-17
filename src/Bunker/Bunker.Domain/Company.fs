namespace Bunker.Domain

module Company =
    open Domain
    open Helpers
    
    let findOwnCompany companyId player =
        match player.JoinedCompanies
              |> List.filter (fun x -> x.IsOwner = true)
              |> List.tryFind (fun x -> x.Company.Id = companyId) with
        | Some companyPlayer -> Ok companyPlayer.Company
        | None -> Fail [ "Company not found" ]
    
    let checkInviteKey company inviteKey =
        if company.JoinKey = inviteKey then Ok()
        else Fail [ "Invite key is invalid" ]
    
    let create name description joinKey player createDate =
        result { 
            let! name = CompanyName.create name
            let! description = CompanyDescription.create description
            let company =
                { Id = CompanyId.NewCompany
                  Name = name
                  Description = description
                  Players = []
                  JoinKey = joinKey
                  Challanges = []
                  CreateDate = createDate
                  Teams = [] }
            
            let companyPlayer =
                { Company = company
                  Player = player
                  IsOwner = true
                  JoinDate = createDate }
            
            return Ok { company with Players = [ companyPlayer ] }
        }
    
    let join company player inviteKey joinDate isOwner =
        result { 
            do! checkInviteKey company inviteKey
            let companyPlayer =
                { Company = company
                  Player = player
                  IsOwner = isOwner
                  JoinDate = joinDate }
            return Ok { company with Players = companyPlayer :: company.Players }
        }
        
    let kick companyId player kickedPlayer =
        let kickFromCompany player (company: Company) =
            Ok { company with Players = company.Players |> List.filter(not << (=) player)}
    
        companyId |>
           (findOwnCompany player >== kickFromCompany kickedPlayer) 
    
    let update player companyId name description =
        let updateCompany name description (company : Company) =
            Ok { company with Name = name
                              Description = description }
        result { let! name = CompanyName.create name
                 let! description = CompanyDescription.create description
                 return companyId |> (findOwnCompany player >== (updateCompany name description)) }
    
    let delete player companyId = companyId |> (findOwnCompany player >== (fun _ -> Ok true))
