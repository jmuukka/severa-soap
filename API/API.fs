namespace Mutex.Visma.Severa.SOAP.API

open System

module private Option =

    let toDateTime since =
        match since with
        | Some value -> value
        | Option.None -> DateTime(2000, 1, 1)

module Contact =

    let private returnArray = Severa.executeReturnArray Factory.createContactClient

    let getChangedContacts invoke context since (options : ContactGetOptions) =
        let since = Option.toDateTime since
        let options = int options
        returnArray invoke context (fun client -> client.GetContactsChangedSince(null, since, options))

module Customer =

    let private returnArray = Severa.executeReturnArray Factory.createCustomerClient
    let private returnSingle = Severa.executeReturnSingle Factory.createCustomerClient

    let getChangedCustomers invoke context since (options : CustomerGetOptions) =
        let since = Option.toDateTime since
        let options = int options
        returnArray invoke context (fun client -> client.GetCustomersChangedSince(since, options))

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetCustomerByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Severa.tryGet

    let add invoke context customer =
        returnSingle invoke context (fun client -> client.AddNewCustomer(customer))

    let update invoke context customer =
        returnSingle invoke context (fun client -> client.UpdateCustomer(customer))
