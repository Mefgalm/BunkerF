namespace Bunker.Domain

module Company =
    open Helpers
    open Domain

    let findOwnCompany companyId player =
        match player.JoinedCompanies |> List.filter (fun x -> x.IsOwner = true) |> List.tryFind (fun x -> x.Company.Id = companyId) with
        | Some companyPlayer -> Ok companyPlayer.Company
        | None -> Fail [ "Company not found" ]
    
    let create name description joinKey player =
        result {
            let! name = CompanyName.create name
            let! description = CompanyDescription.create description                        
        
            return { Id = CompanyId.NewCompany
                     Name = name
                     Description = description
                     Players = []
                     JoinKey = joinKey
                     Teams = [] }
        }
    
    let update player companyId name description =
        let updateCompany name description (company: Company) =
            Ok { company with Name = name
                              Description = description } 
        
        result {
            let! name = CompanyName.create name
            let! description = CompanyDescription.create description                          
        
            return findOwnCompany companyId player 
                    |>= updateCompany name description
        }
