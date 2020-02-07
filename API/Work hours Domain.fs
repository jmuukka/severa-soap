namespace Mutex.Visma.Severa.SOAP.API

open System

type WorkTypeGetOptions =
| None = 0 // include active, include updated and new work types
| ExcludeUpdated = 1
| IncludeInactive = 2

module WorkType =

    let private returnArray = Severa.executeReturnArray Factory.createWorkTypeClient
    let private returnSingle = Severa.executeReturnSingle Factory.createWorkTypeClient

    let getChanged invoke context since (options : WorkTypeGetOptions) =
        let since = Option.toDateTime since
        returnArray invoke context (fun client -> client.GetWorkTypesChangedSince(since, int options))

    //let internal getByPhase invoke context phaseGuid =
    //    returnArray invoke context (fun client -> client.GetWorkTypesByPhaseGUID(phaseGuid))

    let internal getByHourEntry invoke context hourEntryGuid =
        returnSingle invoke context (fun client -> client.GetWorkTypeByHourGUID(hourEntryGuid))

    let getAll invoke context =
        returnArray invoke context (fun client -> client.GetAllWorkTypes())

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetWorkTypeByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Result.mapEntityNotFoundToNone

    let add invoke context workType =
        returnSingle invoke context (fun client -> client.AddNewWorkType(workType))

    let update invoke context workType =
        returnSingle invoke context (fun client -> client.UpdateWorkType(workType))

module WorkHour =

    let private returnArray = Severa.executeReturnArray Factory.createHourEntryClient
    let private returnSingle = Severa.executeReturnSingle Factory.createHourEntryClient
    let private returnBool = Severa.executeReturn Factory.createHourEntryClient

    //let internal getByBusinessUnit invoke context businessUnitGuid startDate endDate =
    //    let startDate = Option.toNullableDateTime startDate
    //    let endDate = Option.toNullableDateTime endDate
    //    returnArray invoke context (fun client -> client.GetHourEntriesByDate(businessUnitGuid, startDate, endDate))

    //let internal getByCriteria invoke context businessUnitGuid startDate endDate userGuid =
    //    let startDate = Option.toNullableDateTime startDate
    //    let endDate = Option.toNullableDateTime endDate
    //    returnArray invoke context (fun client -> client.GetHourEntriesByDateAndUserGUID(businessUnitGuid, startDate, endDate, userGuid))

    //let internal getByCase invoke context caseGuid startDate endDate =
    //    let startDate = Option.toNullableDateTime startDate
    //    let endDate = Option.toNullableDateTime endDate
    //    returnArray invoke context (fun client -> client.GetHourEntriesByCaseGUID(caseGuid, startDate, endDate))

    //let internal getByInvoicedByCase invoke context caseGuid =
    //    returnArray invoke context (fun client -> client.GetInvoicedHourEntriesByCaseGUID(caseGuid))

    let getChanged invoke context since =
        let since = Option.toDateTime since
        returnArray invoke context (fun client -> client.GetHourEntriesChangedSince(since))

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetHourEntryByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Result.mapEntityNotFoundToNone

    let add invoke context hourEntry =
        returnSingle invoke context (fun client -> client.AddNewHourEntry(hourEntry))

    let update invoke context hourEntry =
        returnSingle invoke context (fun client -> client.UpdateHourEntry(hourEntry))

    let delete invoke context guid =
        returnBool invoke context (fun client -> client.DeleteHourEntry(guid))
        |> Result.mapFalseToGeneralError
        |> Result.mapToUnit

    // Work type of hour entry

    let getWorkType invoke context guid =
        WorkType.getByHourEntry invoke context guid

type BelongsTo = 
| InvoicesAndCreditNotes
| Invoices
| CreditNotes

module ReimbursedWorkHour =

    let private returnArray = Severa.executeReturnArray Factory.createHourEntryClient
    let private returnSingle = Severa.executeReturnSingle Factory.createHourEntryClient

    //let internal getReimbursed invoke context businessUnitGuid startDate endDate belongsTo =
    //    let startDate = Option.toNullableDateTime startDate
    //    let endDate = Option.toNullableDateTime endDate
    //    let inCreditNote =
    //        match belongsTo with
    //        | InvoicesAndCreditNotes -> Nullable<bool>()
    //        | Invoices -> Nullable<bool>(false)
    //        | CreditNotes -> Nullable<bool>(true)
    //    returnArray invoke context (fun client -> client.GetReimbursedHourEntriesByDate(businessUnitGuid, startDate, endDate, inCreditNote))

    let getChanged invoke context startDate belongsTo =
        let startDate = Option.toDateTime startDate
        let inCreditNote =
            match belongsTo with
            | InvoicesAndCreditNotes -> Nullable<bool>()
            | Invoices -> Nullable<bool>(false)
            | CreditNotes -> Nullable<bool>(true)
        returnArray invoke context (fun client -> client.GetReimbursedHourEntriesChangedSince(startDate, inCreditNote))

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetReimbursedHourEntryByGUID(guid))

    let tryGetReimbursed invoke context guid =
        get invoke context guid
        |> Result.mapEntityNotFoundToNone

module WorkdayInfo =

    let private returnArray = Severa.executeReturnArray Factory.createHourEntryClient

    let get invoke context businessUnitGuid startDate endDate userGuid includeUnpaidAbsences includeInfoForInactiveUser =
        returnArray invoke context (fun client -> client.GetWorkDayInfoByDateRangeAndUserGUID(businessUnitGuid, startDate, endDate, userGuid, includeUnpaidAbsences, includeInfoForInactiveUser))
