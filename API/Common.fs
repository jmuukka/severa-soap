namespace Mutex.Visma.Severa.SOAP.API

open System

module internal Option =

    let toDateTime since =
        match since with
        | Some value -> value
        | None -> DateTime(2000, 1, 1)

    let toNullableDateTime since =
        match since with
        | Some value -> value
        | None -> Nullable<DateTime>()

module Result =

    let mapNullToEntityNotFound res =
        match res with
        | Ok value ->
            match value with
            | null ->
                Error EntityNotFound
            | _ ->
                res
        | _ ->
            res

    let mapEntityNotFoundToNone res =
        match res with
        | Error err ->
            match err with
            | EntityNotFound
                -> Ok None
            | _
                -> Error err
        | Ok value ->
            Ok (Some value)

    let mapFalseToGeneralError res =
        match res with
        | Ok value ->
            match value with
            | true ->
                Ok value
            | false ->
                Error (General "The operation failed because the API returned false.")
        | Error err ->
            Error err

    let mapToUnit res =
        match res with
        | Ok _ ->
            Ok ()
        | Error err ->
            Error err
