namespace Bunker.Domain

module Player =
    open Domain
    open Helpers
    
    let result = new ResultBuilder()
    
    let create nickName firstName lastName email password isUserExists =
        let check isUserExists =
            if isUserExists then
                Fail ["User already exists"]
            else Ok ()
    
        result { 
            let! nickName = PlayerNickName.create nickName
            let! firstName = PlayerFirstName.create firstName
            let! lastName = PlayerLastName.create lastName
            let! email = Email.create email
            let! password = PlayerPassword.create password
            
            do! (check isUserExists)
            
            let (passwordHash, salt) = Crypto.hashPassword (PlayerPassword.value password)
            
            return { Id = PlayerId.NewPlayer
                     NickName = nickName
                     FirstName = firstName
                     LastName = lastName
                     Email= email
                     PasswordHash= passwordHash
                     PasswordSalt= salt
                     JoinedCompanies = []
                     JoinedTeams = [] }            
        }
    
    let update player nickName firstName =
        result { 
            let! nickName = PlayerNickName.create nickName
            let! firstName = PlayerFirstName.create firstName
            return { player with NickName = nickName
                                 FirstName = firstName }
        }
