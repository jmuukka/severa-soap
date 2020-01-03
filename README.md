# Visma Severa SOAP API

Visma Severa is a project management SaaS product created by Visma Solutions Oy.

Visma Severa has a SOAP API for accessing your own data.

This package contains F# client types and implementation of the API.

## Usage

<pre>
open Xunit
open Mutex.Visma.Severa.SOAP.API

// Compose a new function to use retry logic.
// When Severa API fails then the operation will be retried after 15 seconds.
// The operation will be retried maximum of 3 times.
// The delay is doubled after each retry therefore the retry interval is 15, 30, 60 seconds.
// When the last retry fails then the function will return the Result with the latest failure.
let invoke = Severa.invokeWithRetry 15 3 Severa.invoke

let context =
    {
        ApiKey = ApiKey "YOUR SEVERA SOAP API KEY"
        Binding = Connection.binding
        RemoteAddress = Connection.remoteAddress
    }

[<Fact>]
let ``Get all customers`` () =
    let actual = Customer.getChangedCustomers invoke context None CustomerGetOptions.IncludeInactive

    Assert.ok actual
</pre>

Copyright (c) Jarmo Muukka, Mutex Oy 2020
