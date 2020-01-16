module Mutex.Visma.Severa.SOAP.API.ResultUnitTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

[<Fact>]
let ``mapNullToEntityNotFound with null returns EntityNotFound`` () =
    let result = Ok null

    let actual = Result.mapNullToEntityNotFound result

    Assert.equals (Error EntityNotFound) actual

[<Fact>]
let ``mapNullToEntityNotFound with value returns Ok value`` () =
    let result : Result<string, Failure> = Ok ""

    let actual = Result.mapNullToEntityNotFound result

    Assert.equals result actual

[<Fact>]
let ``mapNullToEntityNotFound with Error returns Error`` () =
    let result : Result<string, Failure> = Error Authentication

    let actual = Result.mapNullToEntityNotFound result

    Assert.equals result actual

[<Fact>]
let ``mapEntityNotFoundToNone with EntityNotFound Error returns Ok None`` () =
    let result : Result<string, Failure> = Error EntityNotFound

    let actual = Result.mapEntityNotFoundToNone result

    Assert.equals (Ok None) actual

[<Fact>]
let ``mapEntityNotFoundToNone with Authentication Error returns Authentication Error`` () =
    let result : Result<string, Failure> = Error Authentication

    let actual = Result.mapEntityNotFoundToNone result

    let expected : Result<string option, Failure> = Error Authentication
    Assert.equals expected actual

[<Fact>]
let ``mapEntityNotFoundToNone with Ok value returns Ok Some value`` () =
    let value = ""
    let result : Result<string, Failure> = Ok value

    let actual = Result.mapEntityNotFoundToNone result

    let expected : Result<string option, Failure> = Ok (Some value)
    Assert.equals expected actual

[<Fact>]
let ``mapFalseToGeneralError with Ok true returns Ok true`` () =
    let result = Ok true

    let actual = Result.mapFalseToGeneralError result

    Assert.equals result actual

[<Fact>]
let ``mapFalseToGeneralError with Ok false returns General Error`` () =
    let result = Ok false

    let actual = Result.mapFalseToGeneralError result

    Assert.generalFailure actual

[<Fact>]
let ``mapFalseToGeneralError with Authentication Error returns Authentication Error`` () =
    let result : Result<bool, Failure> = Error Authentication

    let actual = Result.mapFalseToGeneralError result

    let expected : Result<bool, Failure> = Error Authentication
    Assert.equals expected actual
