module Mutex.Visma.Severa.SOAP.API.CoreTests

open System
open System.ServiceModel
open Xunit
open Mutex.Visma.Severa.SOAP.API

let context = Severa.context ApiKey.load

[<Fact>]
let ``Get all currencies`` () =
    let actual = Severa.executeReturnArray Factory.createCurrencyClient Severa.invoke context (fun client -> client.GetCurrencies())

    Assert.ok actual

[<Fact>]
let ``Get a single currency by ISO code`` () =
    let actual = Severa.executeReturnSingle Factory.createCurrencyClient Severa.invoke context (fun client -> client.GetCurrencyByIsoCode("EUR"))

    Assert.ok actual

[<Fact>]
let ``When invalid guid is used then result should be General Failure Error`` () =
    let actual = Severa.execute Factory.createInvoiceClient Severa.invoke context (fun client -> client.SetInvoiceStatus("guid", "status"))

    Assert.generalFailure actual

[<Fact>]
let ``When non-existing remote address is used then should retry`` () =
    let invoke = Severa.invokeWithRetry 1 2 Severa.invoke
    let nonExistingAddress = "https://severa.visma.com/webservice/S3/API.svc" |> Uri |> EndpointAddress
    let context = { context with RemoteAddress = nonExistingAddress}

    let start = DateTime.UtcNow
    let actual = Severa.execute Factory.createInvoiceClient invoke context (fun client -> client.SetInvoiceStatus("guid", "status"))
    let end' = DateTime.UtcNow

    let minimumWaitTime = 1000
    let maximumWaitTime = minimumWaitTime + 1000 // 1 second is allowed to be elapsed on other things
    let executionTime = int (end' - start).TotalMilliseconds
    Assert.True(executionTime > minimumWaitTime && executionTime < maximumWaitTime)
    Assert.failureException actual

[<Fact>]
let ``Execute multiple commands using a single client`` () =
    use client = Factory.createCurrencyClient context
    use scope = Severa.createContextScope context.ApiKey client

    let actualArray = Severa.invoke client (fun c -> c.GetCurrencies())
    let actualSingle = Severa.invoke client (fun c -> c.GetCurrencyByIsoCode("EUR"))

    Assert.ok actualArray
    Assert.ok actualSingle
