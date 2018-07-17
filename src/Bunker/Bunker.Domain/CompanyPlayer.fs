namespace Bunker.Domain

module CompanyPlayer =
    open Domain
    open Helpers
    
    let join company player inviteKey joinDate isOwner =
        if company.JoinKey = inviteKey then 
            Ok { Company = company
                 Player = player
                 IsOwner = isOwner
                 JoinDate = joinDate }
        else Fail ["Key is invalid"]
