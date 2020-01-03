namespace Mutex.Visma.Severa.SOAP.API

open System
open System.ServiceModel

module Connection =

    let binding =
        let binding = BasicHttpBinding()
        binding.Security.Mode <- BasicHttpSecurityMode.Transport
        let MiB = 1024 * 1024
        let maxSize = 500 * MiB
        binding.MaxReceivedMessageSize <- int64 maxSize
        binding.ReaderQuotas.MaxStringContentLength <- maxSize
        binding.ReceiveTimeout <- new TimeSpan(0, 5, 0)
        binding.SendTimeout <- new TimeSpan(0, 3, 0)
        binding

    let remoteAddress =
        "https://sync.severa.com/webservice/S3/API.svc"
        |> Uri
        |> EndpointAddress
