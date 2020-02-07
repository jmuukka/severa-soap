module ReimbursedWorkHourTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

[<Fact>]
let ``Get all reimbursed work hours`` () =
    
    let actual = ReimbursedWorkHour.getChanged invoke context None BelongsTo.InvoicesAndCreditNotes

    Assert.ok actual
