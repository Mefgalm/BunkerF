namespace Bunker.Domain

open Helpers

module CompanyName =
    type T = CompanyName of string
    
    let create name = Validation.regexCheck "^\w{4,16}$" "Comapny name like ^\w{4,16}$" name CompanyName
    let value (CompanyName e) = e

module CompanyDescription =
    type T = CompanyDescription of string
    
    let create description =
        Validation.regexCheck "^\w{4,16}$" "Company Description like ^\w{4,16}$" description CompanyDescription
    let value (CompanyDescription e) = e
