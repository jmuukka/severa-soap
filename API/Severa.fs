namespace Mutex.Visma.Severa.SOAP.API

open System.ServiceModel

module Severa =

    let getArray<'client, 'channel, 'ret
                  when 'channel : not struct
                   and 'client :> ClientBase<'channel>
                   and 'ret : not struct>
            invoke
            context
            (command : ArrayCommand<'client, 'channel, 'ret>) =
        Soap.executeReturnArray command.CreateClient invoke context command.RequestArray

    let getPaged<'client, 'channel, 'ret
                  when 'channel : not struct
                   and 'client :> ClientBase<'channel>
                   and 'ret : not struct>
            invoke
            context
            (command : PagedCommand<'client, 'channel, 'ret>) =
        let maxCount = command.PageSize
        let rec getPageRec first accumulatedEarlier =
            let result = Soap.executeReturnArray command.CreateClient invoke context (fun client -> command.Request client first maxCount)
            match result with
            | Error _ ->
                result
            | Ok page ->
                let accumulated = Array.append accumulatedEarlier page
                if page.Length < maxCount then
                    Ok accumulated
                else
                    getPageRec (first + page.Length) accumulated

        getPageRec 0 [||]

    let get<'client, 'channel, 'ret
             when 'channel : not struct
              and 'client :> ClientBase<'channel>
              and 'ret : not struct
              and 'ret : null>
            invoke
            context
            (command : Command<'client, 'channel, 'ret>) =
        Soap.executeReturnSingle command.CreateClient invoke context command.Request

    let tryGet<'client, 'channel, 'ret
                when 'channel : not struct
                 and 'client :> ClientBase<'channel>
                 and 'ret : not struct
                 and 'ret : null>
            invoke
            context
            (command : Command<'client, 'channel, 'ret>) =
        get invoke context command
        |> Result.mapEntityNotFoundToNone

    let exec<'client, 'channel, 'ret
              when 'channel : not struct
               and 'client :> ClientBase<'channel>>
            invoke
            context
            (command : Command<'client, 'channel, 'ret>) =
        Soap.executeReturn command.CreateClient invoke context command.Request

    let delete<'client, 'channel
                when 'channel : not struct
                 and 'client :> ClientBase<'channel>>
            invoke
            context
            (command : Command<'client, 'channel, bool>) =
        exec<'client, 'channel, bool> invoke context command
        |> Result.mapFalseToGeneralError
        |> Result.mapToUnit

    /// <summary>Creates a Context value using an API key with default binding and remote address.</summary>
    /// <param name="apiKey">The API key.</param>
    /// <returns>A Context value.</returns>
    let context apiKey =
        {
            ApiKey = ApiKey apiKey
            Binding = Connection.binding
            RemoteAddress = Connection.severaRemoteAddress
        }
