namespace Mutex.Visma.Severa.SOAP.API

module Customer =

    let private returnArray = Severa.executeReturnArray Factory.createCustomerClient
    let private returnSingle = Severa.executeReturnSingle Factory.createCustomerClient

    let getChanged invoke context since (options : CustomerGetOptions) =
        let since = Option.toDateTime since
        returnArray invoke context (fun client -> client.GetCustomersChangedSince(since, int options))

    let getSlice invoke context (options : CustomerGetOptions) first count (criteria : CustomerCriteria) =
        let accountGroupGuidsWasNull = criteria <> null && criteria.AccountGroupGuids = null
        if accountGroupGuidsWasNull then
            // Fix a problem in Severa. Severa will crash when AccountGroupGuids is null.
            criteria.AccountGroupGuids <- [||]
        let result = returnArray invoke context (fun client -> client.GetAllCustomers(int options, first, count, criteria))
        if accountGroupGuidsWasNull then
            // Restore the criteria back to its original state.
            criteria.AccountGroupGuids <- null
        result

    let getAll invoke context options criteria =
        let maxCount = 100 // Documentation says that it will return maximum of 100 customers.
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
        |> Result.mapEntityNotFoundToNone

    let add invoke context customer =
        returnSingle invoke context (fun client -> client.AddNewCustomer(customer))

    let update invoke context customer =
        returnSingle invoke context (fun client -> client.UpdateCustomer(customer))

    // Contacts of customer

    let private returnContactArray = Severa.executeReturnArray Factory.createContactClient

    let getContacts invoke context guid =
        returnContactArray invoke context (fun client -> client.GetContactsByAccountGUID(guid))

    let getContactsChanged invoke context guid since (options : ContactGetOptions) =
        let since = Option.toDateTime since
        returnContactArray invoke context (fun client -> client.GetContactsChangedSince(guid, since, int options))

module Contact =

    let private returnArray = Severa.executeReturnArray Factory.createContactClient
    let private returnSingle = Severa.executeReturnSingle Factory.createContactClient
    let private returnBool = Severa.executeReturn Factory.createContactClient

    let getChanged invoke context since options =
        Customer.getContactsChanged invoke context null since options

    let getByCommunicationMethodValue invoke context method value =
        returnArray invoke context (fun client -> client.GetContactsByCommunicationMethodType(method, value))

    let getByEmailAddress invoke context value =
        getByCommunicationMethodValue invoke context CommunicationMethodType.EmailAddress value

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetContactByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Result.mapEntityNotFoundToNone

    let add invoke context contact =
        returnSingle invoke context (fun client -> client.AddNewContact(contact))

    let update invoke context contact =
        returnSingle invoke context (fun client -> client.UpdateContact(contact))

    let delete invoke context guid =
        returnBool invoke context (fun client -> client.DeleteContact(guid))
        |> Result.mapFalseToGeneralError
        |> Result.mapToUnit
