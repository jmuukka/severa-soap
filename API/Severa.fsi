namespace Mutex.Visma.Severa.SOAP.API

open System.ServiceModel

module Severa =

    val getArray<'client, 'channel, 'ret
                  when 'channel : not struct
                   and 'client :> ClientBase<'channel>
                   and 'ret : not struct>
        :  ('channel -> ('channel -> 'ret array) -> Result<'ret array, Failure>)
        -> Context
        -> ArrayCommand<'client, 'channel, 'ret>
        -> Result<'ret array , Failure>

    val getPaged<'client, 'channel, 'ret
                  when 'channel : not struct
                   and 'client :> ClientBase<'channel>
                   and 'ret : not struct>
        :  ('channel -> ('channel -> 'ret array) -> Result<'ret array, Failure>)
        -> Context
        -> PagedCommand<'client, 'channel, 'ret>
        -> Result<'ret array, Failure>

    val get<'client, 'channel, 'ret
             when 'channel : not struct
              and 'client :> ClientBase<'channel>
              and 'ret : not struct
              and 'ret : null>
        :  ('channel -> ('channel -> 'ret) -> Result<'ret, Failure>)
        -> Context
        -> Command<'client, 'channel, 'ret>
        -> Result<'ret, Failure>

    val tryGet<'client, 'channel, 'ret
                when 'channel : not struct
                 and 'client :> ClientBase<'channel>
                 and 'ret : not struct
                 and 'ret : null>
        :  ('channel -> ('channel -> 'ret) -> Result<'ret, Failure>)
        -> Context
        -> Command<'client, 'channel, 'ret>
        -> Result<'ret option, Failure>

    val exec<'client, 'channel, 'ret
              when 'channel : not struct
               and 'client :> ClientBase<'channel>>
        :  ('channel -> ('channel -> 'ret) -> Result<'ret, Failure>)
        -> Context
        -> Command<'client, 'channel, 'ret>
        -> Result<'ret, Failure>

    val delete<'client, 'channel
              when 'channel : not struct
               and 'client :> ClientBase<'channel>>
        :  ('channel -> ('channel -> bool) -> Result<bool, Failure>)
        -> Context
        -> Command<'client, 'channel, bool>
        -> Result<unit, Failure>

    /// <summary>Creates a Context value using an API key with default binding and remote address.</summary>
    /// <param name="apiKey">The API key.</param>
    /// <returns>A Context value.</returns>
    val context : string -> Context
