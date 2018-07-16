namespace Bunker.Domain

module Helpers =
    open System.Text.RegularExpressions
    
    type Result<'a> =
        | Ok of 'a
        | Fail of string list
    
    let bind f1 f2 =
        match f2 with
        | Ok x -> f1 x
        | Fail error -> Fail error
        
    let (>==) f1 f2 = f1 >> bind f2
    
    let (|>=) r1 f2 = (fun _ -> r1) >== f2 
    
    module Validation =
        
        let regexCheck regex errorMessage value returnFun =
            if not <| Regex.IsMatch(value, regex) then Fail [ errorMessage ]
            else Ok <| returnFun value
    
    type ResultBuilder() =
        
        member this.Bind(x, f) =
            match x with
            | Fail e -> Fail e
            | Ok a -> f a
        
        member this.Return(x) = Ok x
