namespace Mutex.Visma.Severa.SOAP.API

open System
open System.ServiceModel
open System.ServiceModel.Channels

type ApiKey = ApiKey of string

[<NoComparison>]
type Context = {
    ApiKey : ApiKey
    Binding : Binding
    RemoteAddress : EndpointAddress
}

[<NoComparison>]
type Failure =
    | Authentication
    | Permission
    | EntityNotFound
    | Validation of string
    | General of string
    | Exception of exn

type ClientFactory<'client, 'channel when 'channel : not struct and 'client :> ClientBase<'channel>> =
    Context -> 'client

type Invoke<'channel, 'ret> =
    'channel -> ('channel -> 'ret) -> Result<'ret, Failure>

type Exec<'client, 'channel, 'ret when 'channel : not struct and 'client :> ClientBase<'channel> and 'ret : not struct> =
    ClientFactory<'client, 'channel>
        -> Invoke<'client, 'ret>
        -> Context
        -> ('client -> 'ret)
        -> Result<'ret, Failure>

type ExecArray<'client, 'channel, 'ret when 'channel : not struct and 'client :> ClientBase<'channel> and 'ret : not struct> =
    ClientFactory<'client, 'channel>
        -> Invoke<'client, 'ret array>
        -> Context
        -> ('client -> 'ret array)
        -> Result<'ret array, Failure>

type SeveraApiOperationContextScope(channel : IContextChannel,
                                    contractNamespace : string,
                                    apiKey : ApiKey) =

    let mutable disposed = false
    let scope = new OperationContextScope(channel)

    do
        let (ApiKey apiKey) = apiKey
        let header = MessageHeader.CreateHeader("WebServicePassword",
                                                contractNamespace,
                                                apiKey)
        OperationContext.Current.OutgoingMessageHeaders.Add(header)

    let cleanup() =
        if not disposed then
            disposed <- true
            scope.Dispose()

    interface IDisposable with
        member this.Dispose() =
            cleanup()
            GC.SuppressFinalize(this)

    override this.Finalize() =
        cleanup()
