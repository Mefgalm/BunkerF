namespace Bunker.Domain

module Crypto =
    open System.Security.Cryptography
    
    let hashPassword password =
        let rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 16, 10000)
        (rfc2898DeriveBytes.GetBytes(20), rfc2898DeriveBytes.Salt)
    
    let verify (password : string) (salt : byte array) hash =
        (new Rfc2898DeriveBytes(password, salt, 10000)).GetBytes(20)
        |> Array.zip hash
        |> Array.forall (fun (x, y) -> x = y)
