# Visma Severa SOAP API

Visma Severa is a project management SaaS product created by Visma Solutions Oy.

Visma Severa has a SOAP API for accessing your own data.

This package contains easy to use functions for accessing Visma Severa - implemented in F#.

## Usage

<pre>
open Xunit
open Mutex.Visma.Severa.SOAP.API

// Partially apply a function to use retry logic by baking in the parameters.
// When Severa API fails then the operation will be retried after 15 seconds.
// The operation will be retried maximum of 3 times.
// The delay is doubled after each retry therefore the retry interval is 15, 30, 60 seconds.
// When the last retry fails then the function will return the Result with the latest failure.
let invoke = Severa.invokeWithRetry 15 3 Severa.invoke

let context = Severa.context "YOUR SEVERA SOAP API KEY"

[&lt;Fact&gt;]
let ``Get customers changed within one week`` () =
    let since = Some (System.DateTime.UtcNow.AddDays(-7.0))
    let options = CustomerGetOptions.IncludeInactive
    
    let actual = Customer.getChangedCustomers invoke context since options

    Assert.ok actual
</pre>

Copyright (c) Jarmo Muukka, Mutex Oy 2020
