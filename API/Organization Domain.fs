namespace Mutex.Visma.Severa.SOAP.API

module BusinessUnit =

    let private returnArray = Severa.executeReturnArray Factory.createBusinessUnitClient
    let private returnSingle = Severa.executeReturnSingle Factory.createBusinessUnitClient

    let getChanged invoke context guid since (options : BusinessUnitGetOptions) =
        let since = Option.toDateTime since
        returnArray invoke context (fun client -> client.GetBusinessUnitsChangedSince(guid, since, int options))

    let getHierarchy invoke context rootGuid (options : BusinessUnitGetOptions) =
        returnArray invoke context (fun client -> client.GetBusinessUnitHierarchy(rootGuid, int options))

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetBusinessUnitByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Result.mapEntityNotFoundToNone

    let getByCode invoke context code =
        returnSingle invoke context (fun client -> client.GetBusinessUnitByCode(code))

    let tryGetByCode invoke context code =
        getByCode invoke context code
        |> Result.mapEntityNotFoundToNone

    let add invoke context businessUnit =
        returnSingle invoke context (fun client -> client.AddNewBusinessUnit(businessUnit))

    let update invoke context businessUnit =
        returnSingle invoke context (fun client -> client.UpdateBusinessUnit(businessUnit))

    // Users in business unit

    let getChangedUsers invoke context guid since options =
        User.getChangedUsersInBusinessUnit invoke context guid since options 

    let getActiveUsers invoke context guid =
        User.getUsersInBusinessUnit invoke context guid false

    let getUsers invoke context guid =
        User.getUsersInBusinessUnit invoke context guid true