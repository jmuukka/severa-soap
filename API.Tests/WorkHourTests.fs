module WorkHourTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

[<Fact>]
let ``Get all work hours`` () =
    let getChangedWorkHours = WorkHour.getChanged None
    
    let actual = Severa.getArray invoke context getChangedWorkHours

    Assert.ok actual
