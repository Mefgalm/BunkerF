namespace Bunker.Domain

module Challange =
    open Domain
    open Helpers
    
    let create player companyId challangeName challangeDescription =
        match player |> (Company.findCompany companyId) with
        | Ok company -> 
            Ok { Id = NewChallange
                 Name = challangeName
                 Description = challangeDescription
                 Tasks = []
                 Company = company
                 JoinedTeams = [] }
        | Fail errors -> Fail errors
