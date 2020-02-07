[<AutoOpen>]
module Common

open Mutex.Visma.Severa.SOAP.API

let invoke<'a, 'b> : ('a -> ('a -> 'b) -> Result<'b, Failure>) = Severa.invokeWithRetry 10 3 Severa.invoke

let context = Severa.context ApiKey.load
