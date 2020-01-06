﻿namespace Mutex.Visma.Severa.SOAP.API

open System

module internal Option =

    let toDateTime since =
        match since with
        | Some value -> value
        | Option.None -> DateTime(2000, 1, 1)
