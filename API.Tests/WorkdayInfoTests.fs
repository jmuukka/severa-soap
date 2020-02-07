module WorkdayInfoTests

open System
open Xunit
open Mutex.Visma.Severa.SOAP.API

[<Fact>]
let ``Get workday info for null business unit and null user`` () =
    let startDate = DateTime(2000, 1, 1)
    let endDate = DateTime.UtcNow
    
    let actual = WorkdayInfo.get invoke context null startDate endDate null true true

    Assert.ok actual
