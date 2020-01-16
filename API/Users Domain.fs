namespace Mutex.Visma.Severa.SOAP.API

module User =

    let private returnArray = Severa.executeReturnArray Factory.createUserClient
    let private returnSingle = Severa.executeReturnSingle Factory.createUserClient
    let private returnBool = Severa.executeReturn Factory.createUserClient

    let getAllInBusinessUnit invoke context guid includeInactive =
        returnArray invoke context (fun client -> client.GetAllUsers(guid, includeInactive))

    let getAllActive invoke context =
        getAllInBusinessUnit invoke context null false

    let getAll invoke context =
        getAllInBusinessUnit invoke context null true

    let getMany invoke context guids =
        returnArray invoke context (fun client -> client.GetUsersByGUIDs(guids))

    let getChangedUsersInBusinessUnit invoke context guid since (options : UserGetOptions) =
        let since = Option.toDateTime since
        returnArray invoke context (fun client -> client.GetUsersChangedSince(guid, since, int options))

    let getChangedUsers invoke context since options =
        getChangedUsersInBusinessUnit invoke context null since options 

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetUserByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Result.mapEntityNotFoundToNone

    let getByCode invoke context code =
        returnSingle invoke context (fun client -> client.GetUserByCode(code))

    let tryGetByCode invoke context code =
        getByCode invoke context code
        |> Result.mapEntityNotFoundToNone

    let getByName invoke context firstName lastName =
        returnSingle invoke context (fun client -> client.GetUserByName(firstName, lastName))

    let tryGetByName invoke context firstName lastName =
        getByName invoke context firstName lastName
        |> Result.mapEntityNotFoundToNone

    let add invoke context user =
        returnSingle invoke context (fun client -> client.AddNewUser(user))

    let update invoke context user =
        returnSingle invoke context (fun client -> client.UpdateUser(user))

    let disable invoke context guid =
        returnBool invoke context (fun client -> client.DisableUser(guid))
        |> Result.mapFalseToGeneralError
        |> Result.mapToUnit

    let enable invoke context guid =
        returnBool invoke context (fun client -> client.EnableUser(guid))
        |> Result.mapFalseToGeneralError
        |> Result.mapToUnit

    let getEmployments invoke context guid =
        Severa.executeReturnArray Factory.createEmploymentClient invoke context (fun client -> client.GetEmploymentsByUserGUID(guid))

module Employment =

    let private returnSingle = Severa.executeReturnSingle Factory.createEmploymentClient
    let private returnBool = Severa.executeReturn Factory.createEmploymentClient

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetEmploymentByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Result.mapEntityNotFoundToNone

    let add invoke context employment =
        returnSingle invoke context (fun client -> client.AddNewEmployment(employment))

    let update invoke context employment =
        returnSingle invoke context (fun client -> client.UpdateEmployment(employment))

    let delete invoke context guid =
        returnBool invoke context (fun client -> client.DeleteEmployment(guid))
        |> Result.mapFalseToGeneralError
        |> Result.mapToUnit
