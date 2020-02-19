namespace Mutex.Visma.Severa.SOAP.API

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
