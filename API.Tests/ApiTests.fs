module Mutex.Visma.Severa.SOAP.API.ApiTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

let invoke = Severa.invoke

let context = Severa.context ApiKey.load

[<Fact>]
let ``Get changed customers`` () =
    let actual = Customer.getChangedCustomers invoke context None CustomerGetOptions.IncludeInactive

    Assert.ok actual

[<Fact>]
let ``Get all customers`` () =
    let criteria = CustomerCriteria()

    let actual = Customer.getAll invoke context CustomerGetOptions.IncludeInactive criteria

    Assert.ok actual
