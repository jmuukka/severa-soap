namespace Mutex.Visma.Severa.SOAP.API

open System.ServiceModel
open System.ServiceModel.Channels

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

    interface System.IDisposable with
        member this.Dispose() =
            cleanup()
            System.GC.SuppressFinalize(this)

    override this.Finalize() =
        cleanup()
