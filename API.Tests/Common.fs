[<AutoOpen>]
module Common

open Mutex.Visma.Severa.SOAP.API

//let invoke<'a, 'b> : ('a -> ('a -> 'b) -> Result<'b, Failure>) =
//    Soap.invokeWithRetry 10 3 Soap.invoke

let invoke = Soap.invoke

let context = Severa.context ApiKey.load
