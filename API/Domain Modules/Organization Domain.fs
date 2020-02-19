namespace Mutex.Visma.Severa.SOAP.API

module Organization =

    let getBillingAddress =
        Command.forReq Factory.createAddressClient (fun client -> client.GetBillingAddress())

module BusinessUnit =

    let private createClient = Factory.createBusinessUnitClient

    let getChanged guid since (options : BusinessUnitGetOptions) =
        let since = Option.toDateTime since
        Command.forArrayReq createClient (fun client -> client.GetBusinessUnitsChangedSince(guid, since, int options))

    let getHierarchy rootGuid (options : BusinessUnitGetOptions) =
        Command.forArrayReq createClient (fun client -> client.GetBusinessUnitHierarchy(rootGuid, int options))

    let get guid =
        Command.forReq createClient (fun client -> client.GetBusinessUnitByGUID(guid))

    let getByCode code =
        Command.forReq createClient (fun client -> client.GetBusinessUnitByCode(code))

    let add businessUnit =
        Command.forReq createClient (fun client -> client.AddNewBusinessUnit(businessUnit))

    let update businessUnit =
        Command.forReq createClient (fun client -> client.UpdateBusinessUnit(businessUnit))

    // Users in business unit

    //let getChangedUsers guid since options =
    //    User.getChangedUsersInBusinessUnit guid since options 

    //let getActiveUsers guid =
    //    User.getUsersInBusinessUnit guid false

    //let getUsers guid =
    //    User.getUsersInBusinessUnit guid true

module CurrencyRate =

    let private createClient = Factory.createCurrencyClient

    let getAll =
        Command.forArrayReq createClient (fun client -> client.GetCurrencies())

    let get guid =
        Command.forReq createClient (fun client -> client.GetCurrencyByGUID(guid))

    let getByCode code =
        Command.forReq createClient (fun client -> client.GetCurrencyByIsoCode(code))
