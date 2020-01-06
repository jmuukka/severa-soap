module Mutex.Visma.Severa.SOAP.API.ApiTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

let invoke = Severa.invoke

let context = Severa.context ApiKey.load

[<Fact>]
let ``Get customers changed within one week`` () =
    let since = Some (System.DateTime.UtcNow.AddDays(-7.0))
    let options = CustomerGetOptions.IncludeInactive
    
    let actual = Customer.getChangedCustomers invoke context since options

    Assert.ok actual

[<Fact>]
let ``Get all customers`` () =
    let criteria = CustomerCriteria()

    let actual = Customer.getAll invoke context CustomerGetOptions.IncludeInactive criteria

    Assert.ok actual
