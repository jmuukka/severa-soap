module ReimbursedWorkHourTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

[<Fact>]
let ``Get all reimbursed work hours`` () =
    let getChanged = ReimbursedWorkHour.getChanged None BelongsTo.InvoicesAndCreditNotes
    
    let actual = Severa.getArray invoke context getChanged

    Assert.ok actual
