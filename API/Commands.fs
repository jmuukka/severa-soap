namespace Mutex.Visma.Severa.SOAP.API

open System.ServiceModel

[<NoComparison>]
[<NoEquality>]
type Command<'client, 'channel, 'ret
              when 'channel : not struct
               and 'client :> ClientBase<'channel>> = {
    CreateClient : Context -> 'client
    Request : 'channel -> 'ret
}

[<NoComparison>]
[<NoEquality>]
type ArrayCommand<'client, 'channel, 'ret
                   when 'channel : not struct
                    and 'client :> ClientBase<'channel>> = {
    CreateClient : Context -> 'client
    RequestArray : 'channel -> 'ret array
}

[<NoComparison>]
[<NoEquality>]
type PagedCommand<'client, 'channel, 'ret
                   when 'channel : not struct
                   and 'client :> ClientBase<'channel>> = {
    CreateClient : Context -> 'client
    Request : 'channel -> int -> int -> 'ret array
    PageSize : int
}

module Command =

    let forReq<'client, 'channel, 'ret
                when 'channel : not struct
                 and 'client :> ClientBase<'channel>>
            (createClient : Context -> 'client)
            (request : 'channel -> 'ret) =
        {
            CreateClient = createClient
            Request = request
        }

    let forArrayReq<'client, 'channel, 'ret
                     when 'channel : not struct
                      and 'client :> ClientBase<'channel>>
            (createClient : Context -> 'client)
            (request : 'channel -> 'ret array) =
        {
            CreateClient = createClient
            RequestArray = request
        }

    let forPagedReq<'client, 'channel, 'ret
                     when 'channel : not struct
                      and 'client :> ClientBase<'channel>>
            (createClient : Context -> 'client)
            (request : 'channel -> int -> int -> 'ret array)
            pageSize =
        {
            CreateClient = createClient
            Request = request
            PageSize = pageSize
        }
