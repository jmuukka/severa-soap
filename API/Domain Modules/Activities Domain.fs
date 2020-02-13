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

    let private createClient = Factory.createActivityClient

    let getInstances userGuid startsAfter startsBefore endsAfter endsBefore activityTypeGuid customerGuid projectGuid firstRow maxRows =
        Command.forArrayReq createClient (fun client -> client.GetActivityInstances(userGuid, startsAfter, startsBefore, endsAfter, endsBefore, activityTypeGuid, customerGuid, projectGuid, firstRow, maxRows))

    let getByCriteria businessUnitGuid activityCategory startDate endDate =
        let activityCategory =
            match activityCategory with
            | None ->
                null
            | Some (category : ActivityCategory) ->
                category.ToString()
        Command.forArrayReq createClient (fun client -> client.GetActivitiesByDate(startDate, endDate, activityCategory, businessUnitGuid))

    let get guid =
        Command.forReq createClient (fun client -> client.GetActivityByGUID(guid))

    let add activity =
        Command.forReq createClient (fun client -> client.AddNewActivity(activity))

    let update activity =
        Command.forReq createClient (fun client -> client.UpdateActivity(activity))

    let delete guid =
        Command.forReq createClient (fun client -> client.DeleteActivity(guid))

    let getParticipants guid =
        Command.forArrayReq createClient (fun client -> client.GetAllActivityParticipants(guid))

    let getUserParticipants guid =
        Command.forArrayReq createClient (fun client -> client.GetActivityUserParticipants(guid))

    let getContactParticipants guid =
        Command.forArrayReq createClient (fun client -> client.GetActivityContactParticipants(guid))

    let getResourceParticipants guid =
        Command.forArrayReq createClient (fun client -> client.GetActivityResourceParticipants(guid))

    let addUserParticipant activityGuid userGuid (memberStatus : MemberStatus) =
        Command.forReq createClient (fun client -> client.AddNewActivityUserParticipant(activityGuid, userGuid, byte memberStatus))

    let addContactParticipant activityGuid contactGuid =
        Command.forReq createClient (fun client -> client.AddNewActivityContactParticipant(activityGuid, contactGuid))

    let addResourceParticipant activityGuid resourceGuid =
        Command.forReq createClient (fun client -> client.AddNewActivityResourceParticipant(activityGuid, resourceGuid))

    let removeUserParticipant activityGuid userGuid =
        Command.forReq createClient (fun client -> client.RemoveActivityUserParticipant(activityGuid, userGuid))

    let removeContactParticipant activityGuid contactGuid =
        Command.forReq createClient (fun client -> client.RemoveActivityContactParticipant(activityGuid, contactGuid))

    let removeResourceParticipant activityGuid resourceGuid =
        Command.forReq createClient (fun client -> client.RemoveActivityResourceParticipant(activityGuid, resourceGuid))

module ActivityType =

    let private createClient = Factory.createActivityTypeClient

    let getAll =
        Command.forArrayReq createClient (fun client -> client.GetAllActivityTypes())

    let get guid =
        Command.forReq createClient (fun client -> client.GetActivityTypeByGUID(guid))

    let add activityType =
        Command.forReq createClient (fun client -> client.AddNewActivityType(activityType))

    let update activityType =
        Command.forReq createClient (fun client -> client.UpdateActivityType(activityType))

    let delete guid =
        Command.forReq createClient (fun client -> client.DeleteActivityType(guid))
