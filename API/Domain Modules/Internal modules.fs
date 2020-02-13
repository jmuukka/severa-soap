namespace Mutex.Visma.Severa.SOAP.API

module internal PhaseInternal =

    let private createClient = Factory.createPhaseClient

    let getChanged businessUnitGuid since (options : PhaseGetOptions) =
        let since = Option.toDateTime since
        Command.forArrayReq createClient (fun client -> client.GetPhasesChangedSince(businessUnitGuid, since, int options))

    let getAllInProject projectGuid =
        Command.forArrayReq createClient (fun client -> client.GetPhasesByCaseGUID(projectGuid))

module internal ProjectInternal =

    let private createClient = Factory.createCaseClient

    let getChanged businessUnitGuid since (options : CaseGetOptions) =
        let since = Option.toDateTime since
        Command.forArrayReq createClient (fun client -> client.GetCasesChangedSince(businessUnitGuid, since, int options))

    let getAllInBusinessUnit businessUnitGuid criteria =
        Command.forArrayReq createClient (fun client -> client.GetAllCases(businessUnitGuid, criteria))

    let getAllInBusinessUnitByCustomer businessUnitGuid customerGuid =
        Command.forArrayReq createClient (fun client -> client.GetCasesByAccountGUID(businessUnitGuid, customerGuid))
