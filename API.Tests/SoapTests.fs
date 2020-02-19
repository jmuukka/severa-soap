module SoapTests

open System
open System.ServiceModel
open Xunit
open Mutex.Visma.Severa.SOAP.API

[<Fact>]
let ``Get all currencies`` () =
    let actual = Soap.executeReturnArray Factory.createCurrencyClient Soap.invoke context (fun client -> client.GetCurrencies())

    Assert.ok actual

[<Fact>]
let ``Get a single currency by ISO code`` () =
    let actual = Soap.executeReturnSingle Factory.createCurrencyClient Soap.invoke context (fun client -> client.GetCurrencyByIsoCode("EUR"))

    Assert.ok actual

[<Fact>]
let ``When invalid guid is used then result should be General Failure Error`` () =
    let actual = Soap.execute Factory.createInvoiceClient Soap.invoke context (fun client -> client.SetInvoiceStatus("guid", "status"))

    Assert.generalFailure actual

[<Fact>]
let ``When non-existing remote address is used then should retry`` () =
    let invoke = Soap.invokeWithRetry 1 2 Soap.invoke
    let nonExistingAddress = "https://severa.visma.com/webservice/S3/API.svc" |> Uri |> EndpointAddress
    let context = { context with RemoteAddress = nonExistingAddress}

    let start = DateTime.UtcNow
    let actual = Soap.execute Factory.createInvoiceClient invoke context (fun client -> client.SetInvoiceStatus("guid", "status"))
    let end' = DateTime.UtcNow

    let minimumWaitTime = 1000
    let maximumWaitTime = minimumWaitTime + 1000 // 1 second is allowed to be elapsed on other things
    let executionTime = int (end' - start).TotalMilliseconds
    Assert.True(executionTime > minimumWaitTime && executionTime < maximumWaitTime)
    Assert.failureException actual

[<Fact>]
let ``Execute multiple commands using a single client`` () =
    use client = Factory.createCurrencyClient context
    use scope = Soap.createContextScope context.ApiKey client

    let actualArray = Soap.invoke client (fun c -> c.GetCurrencies())
    let actualSingle = Soap.invoke client (fun c -> c.GetCurrencyByIsoCode("EUR"))

    Assert.ok actualArray
    Assert.ok actualSingle
