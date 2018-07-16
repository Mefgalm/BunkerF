namespace Bunker.Domain

module Player =
    open Domain
    open Helpers
    
    let private checkUniqueEmail isEmailExists = 
        if isEmailExists then
           Fail ["User already exists"]
        else Ok ()
        
    let result = new ResultBuilder()
    
    let create nickName firstName lastName email password isUserExists =
        result { 
            let! nickName = PlayerNickName.create nickName
            let! firstName = PlayerFirstName.create firstName
            let! lastName = PlayerLastName.create lastName
            let! email = Email.create email
            let! password = PlayerPassword.create password
            
            do! (checkUniqueEmail isUserExists)
            
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
    
    let update player nickName firstName lastName =
        result { 
            let! nickName = PlayerNickName.create nickName
            let! firstName = PlayerFirstName.create firstName
            let! lastName = PlayerLastName.create lastName
            
            return { player with NickName = nickName
                                 FirstName = firstName
                                 LastName = lastName }
        }
