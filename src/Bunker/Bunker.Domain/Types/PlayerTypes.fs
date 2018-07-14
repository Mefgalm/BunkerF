namespace Bunker.Domain

open Helpers

module PlayerId =
    type T =
        | NewPlayer
        | Id of int
    
    let value e =
        function 
        | NewPlayer -> 0
        | Id x -> x

module PlayerNickName =
    type T = PlayerNickName of string
    
    let create name = Validation.regexCheck "^\w{4,16}$" "PlayerNickName like ^\w{4,16}$" name PlayerNickName
    let value (PlayerNickName e) = e

module PlayerFirstName =
    type T = PlayerFirstName of string
    
    let create name = Validation.regexCheck "^\w{4,16}$" "FirstName like ^\w{4,16}$" name PlayerFirstName
    let value (PlayerFirstName e) = e

module PlayerLastName =
    type T = PlayerLastName of string
    
    let create name = Validation.regexCheck "^\w{4,16}$" "LastName like ^\w{4,16}$" name PlayerLastName
    let value (PlayerLastName e) = e

module PlayerPassword =
    type T = PlayerPassword of string
    
    let create password =
        Validation.regexCheck "^[A-Za-z0-9]{8,16}$" "Password must contains latin symbols and digits. Size 8-16" 
            password PlayerPassword
    let value (PlayerPassword e) = e
