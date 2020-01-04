namespace Mutex.Visma.Severa.SOAP.API

open System

module private Option =

    let toDateTime since =
        match since with
        | Some value -> value
        | Option.None -> DateTime(2000, 1, 1)

module Contact =

    let private returnArray = Severa.executeReturnArray Factory.createContactClient
    let private returnSingle = Severa.executeReturnSingle Factory.createContactClient

    let getContactsOfCustomer invoke context guid =
        returnArray invoke context (fun client -> client.GetContactsByAccountGUID(guid))

    let getChangedContacts invoke context since (options : ContactGetOptions) =
        let since = Option.toDateTime since
        let options = int options
        returnArray invoke context (fun client -> client.GetContactsChangedSince(null, since, options))

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetContactByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Severa.mapEntityNotFoundToNone

    let add invoke context contact =
        returnSingle invoke context (fun client -> client.AddNewContact(contact))

    let update invoke context contact =
        returnSingle invoke context (fun client -> client.UpdateContact(contact))

    //let delete invoke context guid =
    //    Severa.executeReturnSingle Factory.createContactClient invoke context (fun client -> client.DeleteContact(guid))
    //    //|> mapFalseToError

module Customer =

    let private returnArray = Severa.executeReturnArray Factory.createCustomerClient
    let private returnSingle = Severa.executeReturnSingle Factory.createCustomerClient

    let getChangedCustomers invoke context since (options : CustomerGetOptions) =
        let since = Option.toDateTime since
        let options = int options
        returnArray invoke context (fun client -> client.GetCustomersChangedSince(since, options))

    let getRange invoke context (options : CustomerGetOptions) first count (criteria : CustomerCriteria) =
        let options = int options
        if criteria <> null && criteria.AccountGroupGuids = null then
            // Fix the problem in Severa. It will crash when AccountGroupGuids is null.
            criteria.AccountGroupGuids <- [||]
        returnArray invoke context (fun client -> client.GetAllCustomers(options, first, count, criteria))

    let getAll invoke context (options : CustomerGetOptions) criteria =
        let maxCount = 100
        let rec readRange first accumulated =
            let res = getRange invoke context options first maxCount criteria
            match res with
            | Error _ ->
                res
            | Ok currentPage ->
                let customers = Array.append accumulated currentPage
                if currentPage.Length < maxCount then
                    Ok customers
                else
                    readRange (first + currentPage.Length) customers

        readRange 0 [||]

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetCustomerByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Severa.mapEntityNotFoundToNone

    let add invoke context customer =
        returnSingle invoke context (fun client -> client.AddNewCustomer(customer))

    let update invoke context customer =
        returnSingle invoke context (fun client -> client.UpdateCustomer(customer))
