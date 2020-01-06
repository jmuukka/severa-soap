﻿namespace Mutex.Visma.Severa.SOAP.API

module Contact =

    let private returnArray = Severa.executeReturnArray Factory.createContactClient
    let private returnSingle = Severa.executeReturnSingle Factory.createContactClient

    let getContactsOfCustomer invoke context guid =
        returnArray invoke context (fun client -> client.GetContactsByAccountGUID(guid))

    let getChangedContactsOfCustomer invoke context guid since (options : ContactGetOptions) =
        let since = Option.toDateTime since
        let options = int options
        returnArray invoke context (fun client -> client.GetContactsChangedSince(guid, since, options))

    let getChangedContacts invoke context since (options : ContactGetOptions) =
        getChangedContactsOfCustomer invoke context null since options

    let getContactsByCommunicationMethodValue invoke context method value =
        returnArray invoke context (fun client -> client.GetContactsByCommunicationMethodType(method, value))

    let getContactsByEmailAddress invoke context value =
        getContactsByCommunicationMethodValue invoke context CommunicationMethodType.EmailAddress value

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetContactByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Severa.mapEntityNotFoundToNone

    let add invoke context contact =
        returnSingle invoke context (fun client -> client.AddNewContact(contact))

    let update invoke context contact =
        returnSingle invoke context (fun client -> client.UpdateContact(contact))

    let delete invoke context guid =
        Severa.executeReturn Factory.createContactClient invoke context (fun client -> client.DeleteContact(guid))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())

module Customer =

    let private returnArray = Severa.executeReturnArray Factory.createCustomerClient
    let private returnSingle = Severa.executeReturnSingle Factory.createCustomerClient

    let getChangedCustomers invoke context since (options : CustomerGetOptions) =
        let since = Option.toDateTime since
        let options = int options
        returnArray invoke context (fun client -> client.GetCustomersChangedSince(since, options))

    let getSlice invoke context (options : CustomerGetOptions) first count (criteria : CustomerCriteria) =
        let options = int options
        let accountGroupGuidsWasNull = criteria <> null && criteria.AccountGroupGuids = null
        if accountGroupGuidsWasNull then
            // Fix a problem in Severa. Severa will crash when AccountGroupGuids is null.
            criteria.AccountGroupGuids <- [||]
        let result = returnArray invoke context (fun client -> client.GetAllCustomers(options, first, count, criteria))
        if accountGroupGuidsWasNull then
            // Restore the criteria back to its original state.
            criteria.AccountGroupGuids <- null
        result

    let getAll invoke context (options : CustomerGetOptions) criteria =
        let maxCount = 100
        let rec getSliceRec first accumulatedEarlier =
            let res = getSlice invoke context options first maxCount criteria
            match res with
            | Error _ ->
                res
            | Ok slice ->
                let accumulated = Array.append accumulatedEarlier slice
                if slice.Length < maxCount then
                    Ok accumulated
                else
                    getSliceRec (first + slice.Length) accumulated

        getSliceRec 0 [||]

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetCustomerByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Severa.mapEntityNotFoundToNone

    let add invoke context customer =
        returnSingle invoke context (fun client -> client.AddNewCustomer(customer))

    let update invoke context customer =
        returnSingle invoke context (fun client -> client.UpdateCustomer(customer))
