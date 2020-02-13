namespace Mutex.Visma.Severa.SOAP.API

module Phase =

    let private createClient = Factory.createPhaseClient

    let getChanged since options =
        PhaseInternal.getChanged null since options

    let get guid =
        Command.forReq createClient (fun client -> client.GetPhaseByGUID(guid))

    let getByCode projectGuid code =
        Command.forArrayReq createClient (fun client -> client.GetPhasesByCode(code, projectGuid))

    let add phase =
        Command.forReq createClient (fun client -> client.AddNewPhase(phase))

    let update phase =
        Command.forReq createClient (fun client -> client.UpdatePhase(phase))

    let delete guid targetPhaseGuidForItems =
        Command.forReq createClient (fun client -> client.DeletePhase(guid, targetPhaseGuidForItems))

module Project =

    let private createClient = Factory.createCaseClient

    let getAll =
        ProjectInternal.getAllInBusinessUnit null null

    let getByCriteria criteria =
        ProjectInternal.getAllInBusinessUnit null criteria

    let getChanged since options =
        ProjectInternal.getChanged null since options

    let getByPhase phaseGuid =
        Command.forReq createClient (fun client -> client.GetCaseByTaskGUID(phaseGuid))

    let get guid =
        Command.forReq createClient (fun client -> client.GetCaseByGUID(guid))

    let getByNumber number =
        Command.forReq createClient (fun client -> client.GetCaseByNumber(number))

    let add project =
        Command.forReq createClient (fun client -> client.AddNewCase(project))

    let update project =
        Command.forReq createClient (fun client -> client.UpdateCase(project))

    let delete guid =
        Command.forReq createClient (fun client -> client.DeleteCase(guid))

    // Phases of project

    let getPhases guid =
        PhaseInternal.getAllInProject guid

    // Project members

    let getMembers guid =
        Command.forArrayReq createClient (fun client -> client.GetCaseMemberUsersByCaseGUID(guid))

    let addMember projectGuid userGuid =
        Command.forReq createClient (fun client -> client.AddCaseMemberUser(projectGuid, userGuid))
