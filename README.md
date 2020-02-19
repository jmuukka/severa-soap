# Visma Severa SOAP API

Visma Severa is a project management SaaS product created by Visma Solutions Oy.

Visma Severa has a SOAP API for accessing your own data.

This package contains easy to use functions for accessing Visma Severa - implemented in F#.

## Usage

<pre>
open Xunit
open Mutex.Visma.Severa.SOAP.API

// Place things into a module to help understanding the context we are using.
module Severa =

    // Partially apply a function to use retry logic by baking in the parameters.
    // When Severa API fails then the operation will be retried after 15 seconds.
    // The operation will be retried maximum of 3 times.
    // The delay is doubled after each retry therefore the retry interval is 15, 30, 60 seconds.
    // When the last retry fails then the function will return the Result with the latest failure.
    let invoke = Soap.invokeWithRetry 15 3 Soap.invoke

    let context = Severa.context "YOUR SEVERA SOAP API KEY"

    // Let's bake in the first parameters
    // to hide the infrastructure and to simplify the usage.
    let getArray = Severa.getArray invoke context

[&lt;Fact&gt;]
let ``Get customers changed within one week`` () =
    let since = Some (System.DateTime.UtcNow.AddDays(-7.0))
    let options = CustomerGetOptions.IncludeInactive
    let getChangedCustomers = Customer.getChanged since options
    
    let actual = Severa.getArray getChangedCustomers

    Assert.ok actual
</pre>

Copyright (c) Jarmo Muukka, Mutex Oy 2020
