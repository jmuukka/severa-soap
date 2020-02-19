module CustomerTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

let getArray = Severa.getArray invoke context

[<Fact>]
let ``Get customers changed within one week`` () =
    let since = Some (System.DateTime.UtcNow.AddDays(-7.0))
    let options = CustomerGetOptions.IncludeInactive
    let getChanged = Customer.getChanged since options
    
    let actual = getArray getChanged

    Assert.ok actual

[<Fact>]
let ``Get first page of customers`` () =
    let options = CustomerGetOptions.IncludeInactive
    let criteria = CustomerCriteria()
    let getFirstPage = Customer.getPage options criteria 0

    let actual = getArray getFirstPage

    Assert.ok actual

[<Fact>]
let ``Get 10000th page of customers should return empty array`` () =
    let options = CustomerGetOptions.IncludeInactive
    let criteria = CustomerCriteria()
    let getFirstPage = Customer.getPage options criteria 10000

    let actual = getArray getFirstPage

    Assert.valueEquals [||] actual

[<Fact>]
let ``Get all customers`` () =
    let options = CustomerGetOptions.IncludeInactive
    let criteria = CustomerCriteria()
    let getAll = Customer.getAll options criteria

    let actual = Severa.getPaged invoke context getAll

    Assert.ok actual
