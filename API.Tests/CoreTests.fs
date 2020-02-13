module CoreTests

open Xunit
open Mutex.Visma.Severa.SOAP.API

[<Fact>]
let ``Get project by number using executeReturn`` () =
    let createClient = Factory.createCaseClient

    let actual = Soap.executeReturn createClient invoke context (fun client -> client.GetCaseByNumber(1001L))

    Assert.ok actual

[<Fact>]
let ``Get all projects using executeReturnArray`` () =
    let createClient = Factory.createCaseClient

    let actual = Soap.executeReturnArray createClient invoke context (fun client -> client.GetAllCases(null, null))

    Assert.ok actual
