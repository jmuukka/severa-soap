namespace Mutex.Visma.Severa.SOAP.API

open System

type WorkTypeGetOptions =
| None = 0 // include active, include updated and new work types
| ExcludeUpdated = 1
| IncludeInactive = 2

module WorkType =

    let private createClient = Factory.createWorkTypeClient

    let getChanged since (options : WorkTypeGetOptions) =
        let since = Option.toDateTime since
        Command.forArrayReq createClient (fun client -> client.GetWorkTypesChangedSince(since, int options))

    let getByPhase phaseGuid =
        Command.forArrayReq createClient (fun client -> client.GetWorkTypesByPhaseGUID(phaseGuid))

    let internal getByHourEntry hourEntryGuid =
        Command.forReq createClient (fun client -> client.GetWorkTypeByHourGUID(hourEntryGuid))

    let getAll =
        Command.forArrayReq createClient (fun client -> client.GetAllWorkTypes())

    let get guid =
        Command.forReq createClient (fun client -> client.GetWorkTypeByGUID(guid))

    let add workType =
        Command.forReq createClient (fun client -> client.AddNewWorkType(workType))

    let update workType =
        Command.forReq createClient (fun client -> client.UpdateWorkType(workType))

module WorkHour =

    let private createClient = Factory.createHourEntryClient

    let getByBusinessUnit businessUnitGuid startDate endDate =
        let startDate = Option.toNullableDateTime startDate
        let endDate = Option.toNullableDateTime endDate
        Command.forArrayReq createClient (fun client -> client.GetHourEntriesByDate(businessUnitGuid, startDate, endDate))

    let getByCriteria businessUnitGuid startDate endDate userGuid =
        let startDate = Option.toNullableDateTime startDate
        let endDate = Option.toNullableDateTime endDate
        Command.forArrayReq createClient (fun client -> client.GetHourEntriesByDateAndUserGUID(businessUnitGuid, startDate, endDate, userGuid))

    let getByProject invoke projectGuid startDate endDate =
        let startDate = Option.toNullableDateTime startDate
        let endDate = Option.toNullableDateTime endDate
        Command.forArrayReq createClient (fun client -> client.GetHourEntriesByCaseGUID(projectGuid, startDate, endDate))

    let getInvoicedByProject projectGuid =
        Command.forArrayReq createClient (fun client -> client.GetInvoicedHourEntriesByCaseGUID(projectGuid))

    let getChanged since =
        let since = Option.toDateTime since
        Command.forArrayReq createClient (fun client -> client.GetHourEntriesChangedSince(since))

    let get guid =
        Command.forReq createClient (fun client -> client.GetHourEntryByGUID(guid))

    let add hourEntry =
        Command.forReq createClient (fun client -> client.AddNewHourEntry(hourEntry))

    let update hourEntry =
        Command.forReq createClient (fun client -> client.UpdateHourEntry(hourEntry))

    let delete guid =
        Command.forReq createClient (fun client -> client.DeleteHourEntry(guid))

    // Work type of hour entry

    let getWorkType guid =
        WorkType.getByHourEntry guid

type BelongsTo = 
| InvoicesAndCreditNotes
| Invoices
| CreditNotes

module ReimbursedWorkHour =

    let private createClient = Factory.createHourEntryClient

    let getByCriteria businessUnitGuid startDate endDate belongsTo =
        let startDate = Option.toNullableDateTime startDate
        let endDate = Option.toNullableDateTime endDate
        let inCreditNote =
            match belongsTo with
            | InvoicesAndCreditNotes -> Nullable<bool>()
            | Invoices -> Nullable<bool>(false)
            | CreditNotes -> Nullable<bool>(true)
        Command.forArrayReq createClient (fun client -> client.GetReimbursedHourEntriesByDate(businessUnitGuid, startDate, endDate, inCreditNote))

    let getChanged startDate belongsTo =
        let startDate = Option.toDateTime startDate
        let inCreditNote =
            match belongsTo with
            | InvoicesAndCreditNotes -> Nullable<bool>()
            | Invoices -> Nullable<bool>(false)
            | CreditNotes -> Nullable<bool>(true)
        Command.forArrayReq createClient (fun client -> client.GetReimbursedHourEntriesChangedSince(startDate, inCreditNote))

    let get guid =
        Command.forReq createClient (fun client -> client.GetReimbursedHourEntryByGUID(guid))

module WorkdayInfo =

    let private createClient = Factory.createHourEntryClient

    let get businessUnitGuid startDate endDate userGuid includeUnpaidAbsences includeInfoForInactiveUser =
        let parameters = businessUnitGuid, startDate, endDate, userGuid, includeUnpaidAbsences, includeInfoForInactiveUser
        Command.forArrayReq createClient (fun client -> client.GetWorkDayInfoByDateRangeAndUserGUID(parameters))
