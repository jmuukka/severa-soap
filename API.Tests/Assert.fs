module Assert

open Mutex.Visma.Severa.SOAP.API

let equals expected actual = 
    match expected = actual with
    | true -> ()
    | false -> failwith "Values are different."

let ok actual = 
    match actual with
    | Ok _ -> ()
    | Error _ -> failwith "An Ok result was expected, but result was an Error."

let generalFailure actual = 
    match actual with
    | Ok _ -> failwith "An Error Failure.General was expected but result was Ok."
    | Error err ->
        match err with
        | General _ -> ()
        | _ -> failwith "An Error Failure.General was expected but result was an Error with another Failure case."

let failureException actual = 
    match actual with
    | Ok _ -> failwith "An Error Failure.Exception was expected but result was Ok."
    | Error err ->
        match err with
        | Exception _ -> ()
        | _ -> failwith "An Error Failure.Exception was expected but result was an Error with another Failure case."
