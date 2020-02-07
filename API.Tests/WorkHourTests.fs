module WorkHourTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

[<Fact>]
let ``Get all work hours`` () =
    
    let actual = WorkHour.getChanged invoke context None

    Assert.ok actual
