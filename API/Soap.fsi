namespace Mutex.Visma.Severa.SOAP.API

open System.ServiceModel

module Soap =

    val invoke
        :  'channel
        -> ('channel -> 'ret)
        -> Result<'ret, Failure>

    val invokeWithRetry<'channel, 'ret>
        :  int
        -> int
        -> ('channel -> ('channel -> 'ret) -> Result<'ret, Failure>)
        -> 'channel
        -> ('channel -> 'ret)
        -> Result<'ret, Failure>

    val createContextScope<'channel
                            when 'channel : not struct>
        :  ApiKey
        -> ClientBase<'channel>
        -> SeveraApiOperationContextScope

    val executeReturn<'client, 'channel, 'ret
                       when 'channel : not struct
                        and 'client :> ClientBase<'channel>>
        :  (Context -> 'client)
        -> ('channel -> ('channel -> 'ret) -> Result<'ret, Failure>)
        -> Context
        -> ('channel -> 'ret)
        -> Result<'ret, Failure>

    val executeReturnSingle<'client, 'channel, 'ret
                             when 'channel : not struct
                              and 'client :> ClientBase<'channel>
                              and 'ret : not struct
                              and 'ret : null>
        :  (Context -> 'client)
        -> ('channel -> ('channel -> 'ret) -> Result<'ret, Failure>)
        -> Context
        -> ('channel -> 'ret)
        -> Result<'ret, Failure>

    val executeReturnArray<'client, 'channel, 'ret
                            when 'channel : not struct
                             and 'client :> ClientBase<'channel>
                             and 'ret : not struct>
        :  (Context -> 'client)
        -> ('channel -> ('channel -> 'ret array) -> Result<'ret array, Failure>)
        -> Context
        -> ('channel -> 'ret array)
        -> Result<'ret array, Failure>

    val execute<'client, 'channel
                 when 'channel : not struct
                  and 'client :> ClientBase<'channel>>
        :  (Context -> 'client)
        -> ('channel -> ('channel -> unit) -> Result<unit, Failure>)
        -> Context
        -> ('channel -> unit)
        -> Result<unit, Failure>
