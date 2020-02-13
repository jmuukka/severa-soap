namespace Mutex.Visma.Severa.SOAP.API

module Employment =

    let private createClient = Factory.createEmploymentClient

    let getByUser userGuid =
        Command.forArrayReq createClient (fun client -> client.GetEmploymentsByUserGUID(userGuid))

    let get guid =
        Command.forReq createClient (fun client -> client.GetEmploymentByGUID(guid))

    let add userGuid employment =
        Command.forReq createClient (fun client -> client.AddNewEmployment(userGuid, employment))

    let update employment =
        Command.forReq createClient (fun client -> client.UpdateEmployment(employment))

    let delete guid =
        Command.forReq createClient (fun client -> client.DeleteEmployment(guid))

module User =

    let private createClient = Factory.createUserClient

    let internal getUsersInBusinessUnit businessUnitGuid includeInactive =
        Command.forArrayReq createClient (fun client -> client.GetAllUsers(businessUnitGuid, includeInactive))

    let getAllActive =
        getUsersInBusinessUnit null false

    let getAll =
        getUsersInBusinessUnit null true

    let getMany guids =
        Command.forArrayReq createClient (fun client -> client.GetUsersByGUIDs(guids))

    let internal getUsersChangedInBusinessUnit businessUnitGuid since (options : UserGetOptions) =
        let since = Option.toDateTime since
        Command.forArrayReq createClient (fun client -> client.GetUsersChangedSince(businessUnitGuid, since, int options))

    let getChangedUsers since options =
        getUsersChangedInBusinessUnit null since options 

    let get guid =
        Command.forReq createClient (fun client -> client.GetUserByGUID(guid))

    let getByCode code =
        Command.forReq createClient (fun client -> client.GetUserByCode(code))

    let getByName firstName lastName =
        Command.forReq createClient (fun client -> client.GetUserByName(firstName, lastName))

    let add user =
        Command.forReq createClient (fun client -> client.AddNewUser(user))

    let update user =
        Command.forReq createClient (fun client -> client.UpdateUser(user))

    let disable guid =
        Command.forReq createClient (fun client -> client.DisableUser(guid))
        //|> Result.mapFalseToGeneralError
        //|> Result.mapToUnit

    let enable guid =
        Command.forReq createClient (fun client -> client.EnableUser(guid))
        //|> Result.mapFalseToGeneralError
        //|> Result.mapToUnit

    // Employments of user

    let getEmployments guid =
        Employment.getByUser guid
