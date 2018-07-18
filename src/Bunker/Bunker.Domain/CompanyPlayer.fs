namespace Bunker.Domain

module CompanyPlayer =
    open Domain
    open Helpers
    
    let join company player joinDate isOwner =
        Ok { Company = company
             Player = player
             IsOwner = isOwner
             JoinDate = joinDate }
