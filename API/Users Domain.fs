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
        let options = int options
        returnArray invoke context (fun client -> client.GetUsersChangedSince(guid, since, options))

    let getChangedUsers invoke context since options =
        getChangedUsersInBusinessUnit invoke context null since options 

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetUserByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Severa.mapEntityNotFoundToNone

    let getByCode invoke context code =
        returnSingle invoke context (fun client -> client.GetUserByCode(code))

    let tryGetByCode invoke context code =
        getByCode invoke context code
        |> Severa.mapEntityNotFoundToNone

    let getByName invoke context firstName lastName =
        returnSingle invoke context (fun client -> client.GetUserByName(firstName, lastName))

    let tryGetByName invoke context firstName lastName =
        getByName invoke context firstName lastName
        |> Severa.mapEntityNotFoundToNone

    let add invoke context user =
        returnSingle invoke context (fun client -> client.AddNewUser(user))

    let update invoke context user =
        returnSingle invoke context (fun client -> client.UpdateUser(user))

    let disable invoke context guid =
        returnBool invoke context (fun client -> client.DisableUser(guid))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())

    let enable invoke context guid =
        returnBool invoke context (fun client -> client.EnableUser(guid))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())

module Employment =

    let private returnArray = Severa.executeReturnArray Factory.createEmploymentClient
    let private returnSingle = Severa.executeReturnSingle Factory.createEmploymentClient
    let private returnBool = Severa.executeReturn Factory.createEmploymentClient

    let getEmploymentsOfUser invoke context guid =
        returnArray invoke context (fun client -> client.GetEmploymentsByUserGUID(guid))

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetEmploymentByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Severa.mapEntityNotFoundToNone

    let add invoke context employment =
        returnSingle invoke context (fun client -> client.AddNewEmployment(employment))

    let update invoke context employment =
        returnSingle invoke context (fun client -> client.UpdateEmployment(employment))

    let deleyte invoke context guid =
        returnBool invoke context (fun client -> client.DeleteEmployment(guid))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())
