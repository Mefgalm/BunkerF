namespace Bunker.Domain

module CompanyPlayer =
    open Domain
    open Helpers
    
    let create player company joinDate isOwner =
        { Company = company
          Player = player
          IsOwner = isOwner 
          JoinDate = joinDate }