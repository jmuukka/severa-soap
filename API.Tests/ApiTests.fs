module Mutex.Visma.Severa.SOAP.API.ApiTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

let invoke = Severa.invoke

let context = Severa.context ApiKey.load

[<Fact>]
let ``Get customers changed within one week`` () =
    let since = Some (System.DateTime.UtcNow.AddDays(-7.0))
    let options = CustomerGetOptions.IncludeInactive
    let getChanged = Customer.getChanged invoke context
    
    let actual = getChanged since options

    Assert.ok actual

[<Fact>]
let ``Get all customers`` () =
    let criteria = CustomerCriteria()
    let getAll = Customer.getAll invoke context

    let actual = getAll CustomerGetOptions.IncludeInactive criteria

    Assert.ok actual
