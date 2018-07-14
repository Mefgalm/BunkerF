namespace Bunker.Domain

module Domain =
    open Helpers
    open Microsoft.FSharp.Collections
    open System
    open System.Text.RegularExpressions
    
    let result = new ResultBuilder()       
    
    module CompanyId =
        type T =
            | NewCompany
            | Id of int
        
        let value e =
            function 
            | NewCompany -> 0
            | Id x -> x    
    
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
        { Id : PlayerId.T
          NickName : PlayerNickName.T
          FirstName : PlayerFirstName.T
          LastName: PlayerLastName.T
          Email: Email.T
          PasswordHash: byte array
          PasswordSalt: byte array
          JoinedCompanies : CompanyPlayer list
          JoinedTeams : Team list }
    
    and CompanyPlayer =
        { Company: Company
          Player: Player
          IsOwner: bool 
          JoinDate: DateTime }
    
    and Company =
        { Id : CompanyId.T
          Name : CompanyName.T
          Description : CompanyDescription.T
          Players : CompanyPlayer list
          JoinKey : string
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
