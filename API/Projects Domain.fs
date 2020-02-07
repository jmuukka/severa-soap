namespace Mutex.Visma.Severa.SOAP.API

module Project =

    let private returnArray = Severa.executeReturnArray Factory.createCaseClient
    let private returnSingle = Severa.executeReturnSingle Factory.createCaseClient
    let private returnBool = Severa.executeReturn Factory.createCaseClient

    let internal getChanged invoke context businessUnitGuid since (options : CaseGetOptions) =
        let since = Option.toDateTime since
        returnArray invoke context (fun client -> client.GetCasesChangedSince(businessUnitGuid, since, int options))

    let internal getAllInBusinessUnit invoke context businessUnitGuid criteria =
        returnArray invoke context (fun client -> client.GetAllCases(businessUnitGuid, criteria))

    let internal getAllInBusinessUnitByCustomer invoke context businessUnitGuid customerGuid =
        returnArray invoke context (fun client -> client.GetCasesByAccountGUID(businessUnitGuid, customerGuid))

    let getAll invoke context =
        getAllInBusinessUnit invoke context null null

    let getByCriteria invoke context criteria =
        getAllInBusinessUnit invoke context null criteria

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetCaseByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Result.mapEntityNotFoundToNone

    let getByNumber invoke context number =
        returnSingle invoke context (fun client -> client.GetCaseByNumber(number))

    let tryGetByNumber invoke context number =
        getByNumber invoke context number
        |> Result.mapEntityNotFoundToNone

    let getByPhase invoke context phaseGuid =
        returnSingle invoke context (fun client -> client.GetCaseByTaskGUID(phaseGuid))

    let tryGetByPhase invoke context phaseGuid =
        getByPhase invoke context phaseGuid
        |> Result.mapEntityNotFoundToNone

    let add invoke context project =
        returnSingle invoke context (fun client -> client.AddNewCase(project))

    let update invoke context project =
        returnSingle invoke context (fun client -> client.UpdateCase(project))

    let delete invoke context guid =
        returnBool invoke context (fun client -> client.DeleteCase(guid))
        |> Result.mapFalseToGeneralError
        |> Result.mapToUnit

    // Project members

    let private returnUserArray = Severa.executeReturnArray Factory.createCaseClient

    let getMembers invoke context guid =
        returnUserArray invoke context (fun client -> client.GetCaseMemberUsersByCaseGUID(guid))

    let addMember invoke context projectGuid userGuid =
        Severa.execute Factory.createCaseClient invoke context (fun client -> client.AddCaseMemberUser(projectGuid, userGuid))

module Phase =

    let private returnArray = Severa.executeReturnArray Factory.createPhaseClient
    let private returnSingle = Severa.executeReturnSingle Factory.createPhaseClient
    let private returnBool = Severa.executeReturn Factory.createPhaseClient

    let internal getChanged invoke context businessUnitGuid since (options : PhaseGetOptions) =
        let since = Option.toDateTime since
        returnArray invoke context (fun client -> client.GetPhasesChangedSince(businessUnitGuid, since, int options))

    let internal getAllInProject invoke context projectGuid =
        returnArray invoke context (fun client -> client.GetPhasesByCaseGUID(projectGuid))

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetPhaseByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Result.mapEntityNotFoundToNone

    let internal getByCode invoke context projectGuid code =
        returnArray invoke context (fun client -> client.GetPhasesByCode(code, projectGuid))

    let add invoke context phase =
        returnSingle invoke context (fun client -> client.AddNewPhase(phase))

    let update invoke context phase =
        returnSingle invoke context (fun client -> client.UpdatePhase(phase))

    let delete invoke context guid targetPhaseGuidForItems =
        returnBool invoke context (fun client -> client.DeletePhase(guid, targetPhaseGuidForItems))
        |> Result.mapFalseToGeneralError
        |> Result.mapToUnit
