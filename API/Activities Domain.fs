namespace Mutex.Visma.Severa.SOAP.API

type ActivityCategory =
| Absences
| CalendarEntry
| ToDo
| ProjectTask
| Personal

type MemberStatus =
| Unknown = 0
| Invited = 1
| Accepted = 2
| Tentative = 3
| Declined = 4

module Activity =

    let private returnArray = Severa.executeReturnArray Factory.createActivityClient
    let private returnParticipantArray = Severa.executeReturnArray Factory.createActivityClient
    let private returnSingle = Severa.executeReturnSingle Factory.createActivityClient
    let private returnBool = Severa.executeReturn Factory.createActivityClient

    let getInstances invoke context userGuid startsAfter startsBefore endsAfter endsBefore activityTypeGuid customerGuid projectGuid firstRow maxRows =
        returnArray invoke context (fun client -> client.GetActivityInstances(userGuid, startsAfter, startsBefore, endsAfter, endsBefore, activityTypeGuid, customerGuid, projectGuid, firstRow, maxRows))

    let getAllByCriteria invoke context startDate endDate activityCategory businessUnitGuid =
        let activityCategory =
            match activityCategory with
            | None ->
                null
            | Some (category : ActivityCategory) ->
                category.ToString()
        returnArray invoke context (fun client -> client.GetActivitiesByDate(startDate, endDate, activityCategory, businessUnitGuid))

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetActivityByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Severa.mapEntityNotFoundToNone

    let add invoke context activity =
        returnSingle invoke context (fun client -> client.AddNewActivity(activity))

    let update invoke context activity =
        returnSingle invoke context (fun client -> client.UpdateActivity(activity))

    let delete invoke context guid =
        returnBool invoke context (fun client -> client.DeleteActivity(guid))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())

    let getParticipants invoke context guid =
        returnParticipantArray invoke context (fun client -> client.GetAllActivityParticipants(guid))

    let getUserParticipants invoke context guid =
        returnParticipantArray invoke context (fun client -> client.GetActivityUserParticipants(guid))

    let getContactParticipants invoke context guid =
        returnParticipantArray invoke context (fun client -> client.GetActivityContactParticipants(guid))

    let getResourceParticipants invoke context guid =
        returnParticipantArray invoke context (fun client -> client.GetActivityResourceParticipants(guid))

    let addUserParticipant invoke context activityGuid userGuid (memberStatus : MemberStatus) =
        returnBool invoke context (fun client -> client.AddNewActivityUserParticipant(activityGuid, userGuid, byte memberStatus))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())

    let addContactParticipant invoke context activityGuid contactGuid =
        returnBool invoke context (fun client -> client.AddNewActivityContactParticipant(activityGuid, contactGuid))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())

    let addResourceParticipant invoke context activityGuid resourceGuid =
        returnBool invoke context (fun client -> client.AddNewActivityResourceParticipant(activityGuid, resourceGuid))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())

    let removeUserParticipant invoke context activityGuid userGuid =
        returnBool invoke context (fun client -> client.RemoveActivityUserParticipant(activityGuid, userGuid))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())

    let removeContactParticipant invoke context activityGuid contactGuid =
        returnBool invoke context (fun client -> client.RemoveActivityContactParticipant(activityGuid, contactGuid))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())

    let removeResourceParticipant invoke context activityGuid resourceGuid =
        returnBool invoke context (fun client -> client.RemoveActivityResourceParticipant(activityGuid, resourceGuid))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())

module ActivityType =

    let private returnArray = Severa.executeReturnArray Factory.createActivityTypeClient
    let private returnSingle = Severa.executeReturnSingle Factory.createActivityTypeClient
    let private returnBool = Severa.executeReturn Factory.createActivityTypeClient

    let getAll invoke context =
        returnArray invoke context (fun client -> client.GetAllActivityTypes())

    let get invoke context guid =
        returnSingle invoke context (fun client -> client.GetActivityTypeByGUID(guid))

    let tryGet invoke context guid =
        get invoke context guid
        |> Severa.mapEntityNotFoundToNone

    let add invoke context activityType =
        returnSingle invoke context (fun client -> client.AddNewActivityType(activityType))

    let update invoke context activityType =
        returnSingle invoke context (fun client -> client.UpdateActivityType(activityType))

    let delete invoke context guid =
        returnBool invoke context (fun client -> client.DeleteActivityType(guid))
        |> Severa.mapFalseToGeneralError
        |> Result.map (fun _ -> ())
