module Domain 
    open Microsoft.FSharp.Collections
    open System
    open System.Text.RegularExpressions
    
    type CompanyId =
        | NewCompany
        | Id of int
    
    type CompanyName = CompanyName of string
    
    type CompanyDescription = CompanyDescription of string option
    
    type PlayerId =
        | NewPlayer
        | Id of int
    
    type PlayerNickName = PlayerNickName of string
    
    type PlayerFirstName = PlayerFirstName of string
    
    type ChallangeId =
        | NewChallange
        | Id of int
    
    type ChallangeName = ChallangeName of string
    
    type ChallangeDescription = ChallangeDescription of string option
    
    type TaskId =
        | NewTask
        | Id of int
    
    type TaskName = TaskName of string
    
    type TaskDescription = TaskDescription of string
    
    type TaskAnswer = TaskAnswer of string
    
    type TeamId =
        | NewTeam
        | Id of int
    
    type TeamName = TeamName of string
    
    type TeamDescription = TeamDescription of string option
    
    type AnswerId =
        | NewAnswer
        | Id of int
    
    type Player =
        { Id : PlayerId
          NickName : PlayerNickName
          FirstName : PlayerFirstName
          JoinedCompanies : Company list
          JoinedTeams : Team list
          OwnedCompanies : Company list
          OwnedTeams : Team list }
    
    and Company =
        { Id : CompanyId
          Name : CompanyName
          Owners : Player list
          Challanges : Challange list
          JoinedPlayers : Player list
          Teams : Team list }
    
    and Challange =
        { Id : ChallangeId
          Name : ChallangeName
          Description : ChallangeDescription
          Tasks : Task list
          Company : Company
          JoinedTeams : Team list }
    
    and Task =
        { Id : TaskId
          Name : TaskName
          Challange : Challange
          Description : TaskDescription
          Answer : TaskAnswer }
    
    and Team =
        { Id : TeamId
          Name : TeamName
          Owner : Player
          Description : TeamDescription
          Company : Company
          Players : Player list
          Challanges : Challange list }
    
    and Answer =
        { Id : AnswerId
          Task : Task
          Player : Player }
    
    type Result<'a> =
        | Ok of 'a
        | Fail of string list
    
    let bind f1 f2 =
        match f2 with
        | Ok x -> f1 x
        | Fail error -> Fail error
    
    module Validation =
        let regexCheck regex errorMessage value returnFun =
            if not <| Regex.IsMatch(value, regex) then
                Fail [errorMessage]
            else Ok <| returnFun
    
    module Player =        
        
        let createNickName nickName =
            Validation.regexCheck "^\w{1,20}$" "NickName is incorrect" nickName PlayerNickName
            
        let createFirstName firstName =
            Validation.regexCheck "^\w{1,20}$" "FirstName is incorrect" firstName PlayerNickName
    
        let create nickName firstName =
            { Id = NewPlayer
              NickName = nickName
              FirstName = firstName
              JoinedCompanies = []
              JoinedTeams = []
              OwnedCompanies = []
              OwnedTeams = [] }
        
        let update player nickName firstName =
            { player with NickName = nickName
                          FirstName = firstName }
    
    module Company =
        let findCompany companyId player =
            match player.OwnedCompanies |> List.tryFind (fun x -> x.Id = companyId) with
            | Some company -> Ok company
            | None -> Fail [ "Company not found" ]
        
        let create companyName player =
            { Id = NewCompany
              Name = companyName
              Owners = [ player ]
              Challanges = []
              JoinedPlayers = []
              Teams = [] }
        
        let update player companyId companyName =
            companyId |> (findCompany player >> bind (fun company -> Ok { company with Name = companyName }))
        
        let addOwner owner companyId player =
            match owner |> findCompany companyId with
            | Ok company -> 
                if company.Owners |> List.exists ((=) player) then Fail [ "Player already added" ]
                else Ok { company with Owners = player :: company.Owners }
            | Fail errors -> Fail errors
    
    module Team =
        let findTeam player teamId =
            match player.OwnedTeams |> List.tryFind (fun x -> x.Id = teamId) with
            | Some company -> Ok company
            | None -> Fail [ "Team not found" ]
        
        let create player company teamName teamDescription =
            match player.OwnedTeams |> List.exists (fun x -> x.Company = company) with
            | true -> Fail [ "Player can have only one team in company" ]
            | false -> 
                Ok { Id = NewTeam
                     Name = teamName
                     Owner = player
                     Description = teamDescription
                     Company = company
                     Players = []
                     Challanges = [] }
        
        let update player teamId teamName teamDescription =
            match player |> findTeam teamId with
            | Ok team -> 
                Ok { team with Name = teamName
                               Description = teamDescription }
            | Fail errors -> Fail errors
    
    module Challange =
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
    
    module Task =
        let create player challangeId taskName taskDescription taskAnswer =
            match player.OwnedCompanies
                  |> List.collect (fun x -> x.Challanges)
                  |> List.tryFind (fun x -> x.Id = challangeId) with
            | Some challange -> 
                Ok { Id = NewTask
                     Name = taskName
                     Challange = challange
                     Description = taskDescription
                     Answer = taskAnswer }
            | None -> Fail [ "Challange not found" ]
    
    module Answer =
        let create (player : Player) task answer =
            match player.JoinedTeams
                  |> List.collect (fun x -> x.Challanges)
                  |> List.collect (fun x -> x.Tasks)
                  |> List.tryFind ((=) task) with
            | Some task -> 
                if task.Answer = answer then 
                    Ok { Id = NewAnswer
                         Task = task
                         Player = player }
                else Fail [ "Answer is wrong" ]
            | None -> Fail [ "Task not found" ]
