namespace Bunker.Requests

[<CLIMutable>]
type PlayerCreateRequest =
    { NickName : string
      FirstName : string
      LastName : string
      Email : string
      Password : string }

[<CLIMutable>]
type PlayerUpdateRequest =
    { Id : int
      NickName : string
      Name : string }