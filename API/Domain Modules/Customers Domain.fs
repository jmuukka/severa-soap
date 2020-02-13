namespace Mutex.Visma.Severa.SOAP.API

module Customer =

    let private createClient = Factory.createCustomerClient

    let private getCustomers (client : ICustomer)
                             (options : CustomerGetOptions)
                             (criteria : CustomerCriteria)
                             first
                             count =
        let accountGroupGuidsWasNull = criteria <> null &&
                                       criteria.AccountGroupGuids = null
        if accountGroupGuidsWasNull then
            // Fix a problem in Severa. Severa will crash when AccountGroupGuids is null.
            criteria.AccountGroupGuids <- [||]
        let result = client.GetAllCustomers(int options, first, count, criteria)
        if accountGroupGuidsWasNull then
            // Restore the criteria back to its original state.
            criteria.AccountGroupGuids <- null
        result

    // Documentation says that it will return maximum of 100 customers.
    let private pageSize = 100

    let getPage options criteria page =
        let getPage client =
            let first = page * pageSize
            getCustomers client options criteria first pageSize

        Command.forArrayReq createClient getPage

    let getAll options criteria =
        let getPage client =
            getCustomers client options criteria

        Command.forPagedReq createClient getPage pageSize

    let getChanged since (options : CustomerGetOptions) =
        let since = Option.toDateTime since
        Command.forArrayReq createClient (fun client -> client.GetCustomersChangedSince(since, int options))

    let get guid =
        Command.forReq createClient (fun client -> client.GetCustomerByGUID(guid))

    let add customer =
        Command.forReq createClient (fun client -> client.AddNewCustomer(customer))

    let update customer =
        Command.forReq createClient (fun client -> client.UpdateCustomer(customer))

    // Contacts of customer

    let getContacts guid =
        Command.forArrayReq Factory.createContactClient (fun client -> client.GetContactsByAccountGUID(guid))

    let getChangedContacts guid since (options : ContactGetOptions) =
        let since = Option.toDateTime since
        Command.forArrayReq Factory.createContactClient (fun client -> client.GetContactsChangedSince(guid, since, int options))

module Contact =

    let private createClient = Factory.createContactClient

    let getChanged since options =
        Customer.getChangedContacts null since options

    let getByCommunicationMethodValue method value =
        Command.forArrayReq createClient (fun client -> client.GetContactsByCommunicationMethodType(method, value))

    let getByEmailAddress emailAddress =
        getByCommunicationMethodValue CommunicationMethodType.EmailAddress emailAddress

    let get guid =
        Command.forReq createClient (fun client -> client.GetContactByGUID(guid))

    let add contact =
        Command.forReq createClient (fun client -> client.AddNewContact(contact))

    let update contact =
        Command.forReq createClient (fun client -> client.UpdateContact(contact))

    let delete guid =
        Command.forReq createClient (fun client -> client.DeleteContact(guid))
