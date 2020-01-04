module Mutex.Visma.Severa.SOAP.API.SeveraUnitTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

[<Fact>]
let ``mapNullToEntityNotFound with null returns EntityNotFound`` () =
    let result = Ok null

    let actual = Severa.mapNullToEntityNotFound result

    Assert.entityNotFound actual

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
let ``mapFalseToGeneralError with Ok true returns Ok unit`` () =
    let result = Ok true

    let actual = Severa.mapFalseToGeneralError result

    let expected : Result<unit, Failure> = Ok ()
    Assert.equals expected actual

[<Fact>]
let ``mapFalseToGeneralError with Ok false returns General Error`` () =
    let result = Ok false

    let actual = Severa.mapFalseToGeneralError result

    Assert.generalFailure actual

[<Fact>]
let ``mapFalseToGeneralError with Authentication Error returns Authentication Error`` () =
    let result : Result<bool, Failure> = Error Authentication

    let actual = Severa.mapFalseToGeneralError result

    let expected : Result<unit, Failure> = Error Authentication
    Assert.equals expected actual
