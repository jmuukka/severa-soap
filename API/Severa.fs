namespace Mutex.Visma.Severa.SOAP.API

open System.ServiceModel
open System.Threading

module private Array =

    let emptyWhenNull arr =
        match arr with
        | null -> [||]
        | _ -> arr

module Severa =

    let invoke client run =
        try
            run client
            |> Ok
        with
            | :? FaultException<AuthenticationFailure> ->
                Error Authentication
            | :? FaultException<NoPermissionFailure> ->
                Error Permission
            | :? FaultException<EntityNotFoundFailure> ->
                Error EntityNotFound
            | :? FaultException<ValidationFailure> as ex ->
                Error (Validation ex.Message)
            //| :? FaultException<QuotaLimitExceededException> ->
            //    Error QuotaLimitExceeded
            //| :? FaultException<TosNotApprovedException> ->
            //    Error TermsOfServiceNotApproved
            | :? FaultException<GeneralFailure> as ex ->
                Error (General ex.Message)
            | :? FaultException<SeveraApiFailure> as ex -> // this is an abstract base class in Severa. All uncaught failures will be caught in here.
                Error (General ex.Message)
            | ex ->
                Error (Exception ex)

    let invokeWithRetry firstWaitTimeInSeconds maxRetryCount invoke client run =
        let rec invokeRec tryCount nextTimeout =
            match invoke client run with
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
                    if tryCount >= maxRetryCount then
                        Error err
                    else
                        Thread.Sleep (nextTimeout * 1000)
                        invokeRec (tryCount + 1) (nextTimeout * 2)

        invokeRec 1 firstWaitTimeInSeconds

    let invokeArray invoke client run =
        invoke client run
        |> Result.map Array.emptyWhenNull

    let createContextScope apiKey (client : ClientBase<'channel>) =
        new SeveraApiOperationContextScope(client.InnerChannel,
                                           client.Endpoint.Contract.Namespace,
                                           apiKey)

    let executeReturn<'client, 'channel, 'ret when 'channel : not struct and 'client :> ClientBase<'channel> and 'ret : not struct> : Exec<'client, 'channel, 'ret> =
        fun createClient invoke context run ->
            use client = createClient context
            use scope = createContextScope context.ApiKey client
            invoke client run

    let executeReturnSingle<'client, 'channel, 'ret when 'channel : not struct and 'client :> ClientBase<'channel> and 'ret : not struct> : Exec<'client, 'channel, 'ret> =
        fun createClient invoke context run ->
            executeReturn createClient invoke context run

    let executeReturnArray<'client, 'channel, 'ret when 'channel : not struct and 'client :> ClientBase<'channel> and 'ret : not struct> : ExecArray<'client, 'channel, 'ret> =
        fun createClient invoke context run ->
            executeReturn createClient (invokeArray invoke) context run

    let execute<'client, 'channel when 'channel : not struct and 'client :> ClientBase<'channel>> : Exec<'client, 'channel, unit> =
        fun createClient invoke context run ->
            executeReturn createClient invoke context run

    let tryGet res =
        match res with
        | Ok v ->
            match v with
            | null ->
                Ok None
            | _ ->
                Ok(Some v)
        | Error err ->
            match err with
            | EntityNotFound
                -> Ok None
            | _
                -> Error err

    let context key =
        {
            ApiKey = ApiKey key
            Binding = Connection.binding
            RemoteAddress = Connection.remoteAddress
        }
