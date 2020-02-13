namespace Mutex.Visma.Severa.SOAP.API

open System.ServiceModel
open System.Threading

module private Array =
    let emptyWhenNull arr =
        match arr with
        | null -> [||]
        | _ -> arr

module Soap =

    let invoke (channel : 'channel) (request : 'channel -> 'ret) =
        try
            request channel
            |> Ok
        with
            | :? FaultException<AuthenticationFailure> ->
                Error Authentication
            | :? FaultException<NoPermissionFailure> ->
                Error Permission
            | :? FaultException<EntityNotFoundFailure> ->
                Error EntityNotFound
            | :? FaultException<ValidationFailure> as ex ->
                Error (Validation ex.Detail.Description)
            //| :? FaultException<QuotaLimitExceededException> ->
            //    Error QuotaLimitExceeded
            //| :? FaultException<TosNotApprovedException> ->
            //    Error TermsOfServiceNotApproved
            | :? FaultException<GeneralFailure> as ex ->
                Error (General ex.Detail.Description)
            | :? FaultException<SeveraApiFailure> as ex -> // this is an abstract base class in Severa. All uncaught failures will be caught in here.
                Error (General ex.Detail.Description)
            | ex ->
                Error (Exception ex)

    let invokeWithRetry<'channel, 'ret>
            firstWaitTimeInSeconds
            maxTryCount
            (invoke : 'channel -> ('channel -> 'ret) -> Result<'ret, Failure>)
            client
            request =
        let rec invokeRec tryCount waitTimeInSeconds =
            match invoke client request with
            | Ok v ->
                Ok v
            | Error err ->
                match err with
                | Authentication
                | Permission
                | EntityNotFound
                | Validation _
                | General _ ->
                    // These are expected to fail in the near future.
                    Error err
                | Exception _ ->
                    // This error may be temporary so we can retry
                    // or return the latest error.
                    if tryCount >= maxTryCount then
                        Error err
                    else
                        Thread.Sleep (waitTimeInSeconds * 1000)
                        invokeRec (tryCount + 1) (waitTimeInSeconds * 2)

        invokeRec 1 firstWaitTimeInSeconds

    let createContextScope apiKey (client : ClientBase<'channel>) =
        new SeveraApiOperationContextScope(client.InnerChannel,
                                           client.Endpoint.Contract.Namespace,
                                           apiKey)

    let executeReturn<'client, 'channel, 'ret
                       when 'channel : not struct
                        and 'client :> ClientBase<'channel>>
        (createClient : Context -> 'client)
        (invoke : 'channel -> ('channel -> 'ret) -> Result<'ret, Failure>)
        context
        (request : 'channel -> 'ret) =
            use client = createClient context
            use scope = createContextScope context.ApiKey client
            let channel = (client :> obj) :?> 'channel
            invoke channel request

    let executeReturnSingle<'client, 'channel, 'ret
                             when 'channel : not struct
                              and 'client :> ClientBase<'channel>
                              and 'ret : not struct
                              and 'ret : null>
        (createClient : Context -> 'client)
        (invoke : 'channel -> ('channel -> 'ret) -> Result<'ret, Failure>)
        context
        (request : 'channel -> 'ret) =
            executeReturn createClient invoke context request
            |> Result.mapNullToEntityNotFound
            // Severa will return null from some endpoints when data was not found.

    let executeReturnArray<'client, 'channel, 'ret
                            when 'channel : not struct
                            and 'client :> ClientBase<'channel>
                            and 'ret : not struct>
        (createClient : Context -> 'client)
        (invoke : 'channel -> ('channel -> 'ret array) -> Result<'ret array, Failure>)
        context
        (request : 'channel -> 'ret array) =
            executeReturn createClient invoke context request
            |> Result.map Array.emptyWhenNull
            // Severa will return null from some endpoints when response would contain zero entries.

    let execute<'client, 'channel
                 when 'channel : not struct
                  and 'client :> ClientBase<'channel>>
        (createClient : Context -> 'client)
        (invoke : 'channel -> ('channel -> unit) -> Result<unit, Failure>)
        context
        (request : 'channel -> unit) =
            executeReturn createClient invoke context request
