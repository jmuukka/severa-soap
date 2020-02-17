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

module Account =

    let private createClient = Factory.createAccountClient

    let add account company address contact =
        Command.forReq createClient (fun client -> client.AddNewAccount(account, company, address, contact))

    let get guid =
        Command.forReq createClient (fun client -> client.GetAccountByGUID(guid))

    let getByName name =
        Command.forReq createClient (fun client -> client.GetAccountByName(name))

    let getByNumber number =
        Command.forReq createClient (fun client -> client.GetAccountByNumber(number))

    let getByVatNumber vatNumber =
        Command.forReq createClient (fun client -> client.GetAccountByVatNumber(vatNumber))

    let getByCompany companyGuid =
        Command.forArrayReq createClient (fun client -> client.GetAccountsByCompanyGUID(companyGuid))

    let getByInsertDate startDate endDate =
        let endDate = Option.toNullableDateTime endDate
        Command.forArrayReq createClient (fun client -> client.GetAccountsByInsertDate(startDate, endDate))

    let getChanged startDate (options : AccountGetOptions) =
        Command.forArrayReq createClient (fun client -> client.GetAccountsChangedSince(startDate, int options))

    let getChangedOpts startDate options =
        Command.forArrayReq createClient (fun client -> client.GetAccountsChangedSinceOpts(startDate, options))

    let getAll index count options =
        Command.forArrayReq createClient (fun client -> client.GetAllAccounts(options, index, count))

    let getHeadOfficeAddress guid =
        Command.forReq createClient (fun client -> client.GetHeadOfficeAddressByAccountGUID(guid))

    let update account =
        Command.forReq createClient (fun client -> client.UpdateAccount(account))

module AccountGroup =

    let private createClient = Factory.createAccountGroupClient

    let add accountGroup =
        Command.forReq createClient (fun client -> client.AddNewAccountGroup(accountGroup))

    let addMembersToAccount accountGuid accountGroupGuids =
        Command.forReq createClient (fun client -> client.AddNewAccountGroupMembers(accountGuid, accountGroupGuids))

    let getAll (options : AccountGroupGetOptions) =
        Command.forArrayReq createClient (fun client -> client.GetAccountGroups(int options))

    let getAllByAccount accountGuid (options : AccountGroupGetOptions) =
        Command.forArrayReq createClient (fun client -> client.GetAccountGroupsByAccountGUID(accountGuid, int options))

    let removeMembersFromAccount accountGuid accountGroupGuids =
        Command.forReq createClient (fun client -> client.RemoveAccountGroupMembers(accountGuid, accountGroupGuids))

type AddressGetOptions =
| NewOnly = 1
| IncludeAddressesOfInactiveAccounts = 2
| ExcludeAddressesOfAccountsWithoutAccountNumber = 4
| ExcludeAddressesOfAccountsWithoutCaseInWonSalesState = 8

module Address =

    let private createClient = Factory.createAddressClient

    let add address =
        Command.forReq createClient (fun client -> client.AddNewAddress(address))

    let get guid =
        Command.forReq createClient (fun client -> client.GetAddressByGUID(guid))

    let getChanged accountGuid startDate (options : AddressGetOptions) =
        Command.forArrayReq createClient (fun client -> client.GetAddressesChangedSince(accountGuid, startDate, int options))

    // In wrong place!
    let getBillingAddressOfOrganization =
        Command.forReq createClient (fun client -> client.GetBillingAddress())

    let getFirstBillingAddressOfAccount accountGuid =
        Command.forReq createClient (fun client -> client.GetFirstAccountsBillingAddress(accountGuid))

    let update address =
        Command.forReq createClient (fun client -> client.UpdateAddress(address))

module Company =

    let private createClient = Factory.createCompanyClient

    let get guid =
        Command.forReq createClient (fun client -> client.GetCompanyByGUID(guid))

module Industry =

    let private createClient = Factory.createIndustryClient

    let add industry =
        Command.forReq createClient (fun client -> client.AddNewIndustry(industry))

    let getAll =
        Command.forArrayReq createClient (fun client -> client.GetIndustries())
