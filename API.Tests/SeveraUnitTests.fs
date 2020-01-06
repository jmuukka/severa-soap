module Mutex.Visma.Severa.SOAP.API.SeveraUnitTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

[<Fact>]
let ``mapNullToEntityNotFound with null returns EntityNotFound`` () =
    let result = Ok null

    let actual = Severa.mapNullToEntityNotFound result

    Assert.equals (Error EntityNotFound) actual

[<Fact>]
let ``mapNullToEntityNotFound with value returns Ok value`` () =
    let result : Result<string, Failure> = Ok ""

    let actual = Severa.mapNullToEntityNotFound result

    Assert.equals result actual

[<Fact>]
let ``mapNullToEntityNotFound with Error returns Error`` () =
    let result : Result<string, Failure> = Error Authentication

    let actual = Severa.mapNullToEntityNotFound result

    Assert.equals result actual

[<Fact>]
let ``mapEntityNotFoundToNone with EntityNotFound Error returns Ok None`` () =
    let result : Result<string, Failure> = Error EntityNotFound

    let actual = Severa.mapEntityNotFoundToNone result

    Assert.equals (Ok None) actual

[<Fact>]
let ``mapEntityNotFoundToNone with Authentication Error returns Authentication Error`` () =
    let result : Result<string, Failure> = Error Authentication

    let actual = Severa.mapEntityNotFoundToNone result

    let expected : Result<string option, Failure> = Error Authentication
    Assert.equals expected actual

[<Fact>]
let ``mapEntityNotFoundToNone with Ok value returns Ok Some value`` () =
    let value = ""
    let result : Result<string, Failure> = Ok value

    let actual = Severa.mapEntityNotFoundToNone result

    let expected : Result<string option, Failure> = Ok (Some value)
    Assert.equals expected actual

[<Fact>]
let ``mapFalseToGeneralError with Ok true returns Ok true`` () =
    let result = Ok true

    let actual = Severa.mapFalseToGeneralError result

    Assert.equals result actual

[<Fact>]
let ``mapFalseToGeneralError with Ok false returns General Error`` () =
    let result = Ok false

    let actual = Severa.mapFalseToGeneralError result

    Assert.generalFailure actual

[<Fact>]
let ``mapFalseToGeneralError with Authentication Error returns Authentication Error`` () =
    let result : Result<bool, Failure> = Error Authentication

    let actual = Severa.mapFalseToGeneralError result

    let expected : Result<bool, Failure> = Error Authentication
    Assert.equals expected actual

type Client = {
    GetSomeString : unit -> string
}

[<Fact>]
let ``invoke: when run does not throw an exception then it returns Ok value`` () =
    let value = ""
    let client = {
        GetSomeString = fun _ -> value
    }
    let run (c : Client) = c.GetSomeString()

    let actual = Severa.invoke client run

    Assert.equals (Ok value) actual

[<Fact>]
let ``invoke: when run throws an AuthenticationFailure exception then it returns Authentication Error`` () =
    let detail = new AuthenticationFailure()
    let exn = System.ServiceModel.FaultException<AuthenticationFailure>(detail)
    let run _ = raise exn

    let actual = Severa.invoke () run

    Assert.equals (Error Authentication) actual

[<Fact>]
let ``invoke: when run throws a NoPermissionFailure exception then it returns Permission Error`` () =
    let detail = new NoPermissionFailure()
    let exn = System.ServiceModel.FaultException<NoPermissionFailure>(detail)
    let run _ = raise exn

    let actual = Severa.invoke () run

    Assert.equals (Error Permission) actual

[<Fact>]
let ``invoke: when run throws an EntityNotFoundFailure exception then it returns EntityNotFound Error`` () =
    let detail = new EntityNotFoundFailure()
    let exn = System.ServiceModel.FaultException<EntityNotFoundFailure>(detail)
    let run _ = raise exn

    let actual = Severa.invoke () run

    Assert.equals (Error EntityNotFound) actual

[<Fact>]
let ``invoke: when run throws a ValidationFailure exception then it returns Validation Error`` () =
    let detail = new ValidationFailure()
    let msg = "X is required"
    detail.Description <- msg
    let exn = System.ServiceModel.FaultException<ValidationFailure>(detail)
    let run _ = raise exn

    let actual = Severa.invoke () run

    Assert.equals (Error (Validation msg)) actual

[<Fact>]
let ``invoke: when run throws a GeneralFailure exception then it returns General Error`` () =
    let detail = new GeneralFailure()
    let msg = "sorry but our service has a bug"
    detail.Description <- msg
    let exn = System.ServiceModel.FaultException<GeneralFailure>(detail)
    let run _ = raise exn

    let actual = Severa.invoke () run

    Assert.equals (Error (General msg)) actual

[<Fact>]
let ``invoke: when run throws an exception then it returns Exception Error`` () =
    let exn = System.NullReferenceException()
    let run _ = raise exn

    let actual = Severa.invoke () run

    Assert.equals (Error (Exception exn)) actual

[<Fact>]
let ``invokeWithRetry: when invoke returns Ok value then it returns Ok value`` () =
    let value = ""
    let waitTime = 10
    let retryCount = 5
    let invoke client run = Ok value

    let actual = Severa.invokeWithRetry waitTime retryCount invoke () ()

    Assert.equals (Ok value) actual

[<Fact>]
let ``invokeWithRetry: when invoke returns Authentication Error then it returns Authentication Error`` () =
    let waitTime = 10
    let retryCount = 5
    let invoke client run = Error Authentication

    let actual = Severa.invokeWithRetry waitTime retryCount invoke () ()

    Assert.equals (Error Authentication) actual

[<Fact>]
let ``invokeWithRetry: when invoke returns Permission Error then it returns Permission Error`` () =
    let waitTime = 10
    let retryCount = 5
    let invoke client run = Error Permission

    let actual = Severa.invokeWithRetry waitTime retryCount invoke () ()

    Assert.equals (Error Permission) actual

[<Fact>]
let ``invokeWithRetry: when invoke returns EntityNotFound Error then it returns EntityNotFound Error`` () =
    let waitTime = 10
    let retryCount = 5
    let invoke client run = Error EntityNotFound

    let actual = Severa.invokeWithRetry waitTime retryCount invoke () ()

    Assert.equals (Error EntityNotFound) actual

[<Fact>]
let ``invokeWithRetry: when invoke returns Validation Error then it returns Validation Error`` () =
    let waitTime = 10
    let retryCount = 5
    let invoke client run = Error (Validation "")

    let actual = Severa.invokeWithRetry waitTime retryCount invoke () ()

    Assert.equals (Error (Validation "")) actual

[<Fact>]
let ``invokeWithRetry: when invoke returns General Error then it returns General Error`` () =
    let waitTime = 10
    let retryCount = 5
    let invoke client run = Error (General "")

    let actual = Severa.invokeWithRetry waitTime retryCount invoke () ()

    Assert.equals (Error (General "")) actual

[<Fact>]
let ``invokeWithRetry: when invoke returns Exception Error every time then it retries and returns Exception Error`` () =
    let waitTime = 1
    let retryCount = 3
    let exn = System.Exception()
    let mutable tryCount = 0
    let invoke client run =
        tryCount <- tryCount + 1
        Error (Exception exn)
    
    let started = System.DateTime.UtcNow
    let actual = Severa.invokeWithRetry waitTime retryCount invoke () ()
    let ended = System.DateTime.UtcNow
    
    Assert.equals (Error (Exception exn)) actual
    Assert.equals retryCount tryCount
    let elapsed = (ended - started).TotalMilliseconds |> int
    let minWaitTime = 1000 + 2000
    let maxWaitTime = minWaitTime + 200
    Assert.True(elapsed > minWaitTime && elapsed < maxWaitTime)

[<Fact>]
let ``invokeWithRetry: when invoke returns Exception Error first time and Ok second time then it returns Ok`` () =
    let waitTime = 1
    let retryCount = 3
    let exn = System.Exception()
    let mutable tryCount = 0
    let invoke client run =
        tryCount <- tryCount + 1
        if tryCount = 1 then
            Error (Exception exn)
        else
            Ok ""
    
    let actual = Severa.invokeWithRetry waitTime retryCount invoke () ()
    
    Assert.equals (Ok "") actual
    Assert.equals 2 tryCount

[<Fact>]
let ``execute: when invoke returns Ok () then function returns Ok ()`` () =
    let createClient ctx = new APIClient(ctx.Binding, ctx.RemoteAddress)
    let invoke client run = Ok (run client)
    let context = Severa.context "api key"
    
    let actual = Severa.execute createClient invoke context (fun _ -> ())
    
    Assert.equals (Ok ()) actual

[<Fact>]
let ``executeReturnArray: when invoke returns Ok array then function returns Ok array`` () =
    let createClient ctx = new APIClient(ctx.Binding, ctx.RemoteAddress)
    let invoke client run = Ok (run client)
    let context = Severa.context "api key"
    
    let actual = Severa.executeReturnArray createClient invoke context (fun _ -> [|""|])
    
    Assert.equals (Ok [|""|]) actual

[<Fact>]
let ``executeReturnArray: when invoke returns Ok null array then function returns Ok array`` () =
    let createClient ctx = new APIClient(ctx.Binding, ctx.RemoteAddress)
    let invoke client run = Ok (run client)
    let context = Severa.context "api key"
    
    let actual = Severa.executeReturnArray createClient invoke context (fun _ -> null)
    
    Assert.equals (Ok [||]) actual

[<Fact>]
let ``executeReturnSingle: when invoke returns Ok string then function returns Ok string`` () =
    let createClient ctx = new APIClient(ctx.Binding, ctx.RemoteAddress)
    let invoke client run = Ok (run client)
    let context = Severa.context "api key"
    
    let actual = Severa.executeReturnSingle createClient invoke context (fun _ -> "")
    
    Assert.equals (Ok "") actual

[<Fact>]
let ``executeReturnSingle: when invoke returns Ok null then function returns EntityNotFound Error`` () =
    let createClient ctx = new APIClient(ctx.Binding, ctx.RemoteAddress)
    let invoke client run = Ok (run client)
    let context = Severa.context "api key"
    
    let actual = Severa.executeReturnSingle createClient invoke context (fun _ -> null)
    
    Assert.equals (Error EntityNotFound) actual
