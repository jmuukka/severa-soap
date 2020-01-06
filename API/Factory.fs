namespace Mutex.Visma.Severa.SOAP.API

module Factory =

    // Note that these domain boundaries are not real.
    // They are invented by the author of this library.

    // ----------------------------------------
    // Internationalization domain
    // ----------------------------------------

    let createCountryClient context =
        new CountryClient(context.Binding, context.RemoteAddress)

    let createLanguageClient context =
        new LanguageClient(context.Binding, context.RemoteAddress)

    let createTimezoneClient context =
        new TimezoneClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Organization domain
    // ----------------------------------------

    let createBusinessUnitClient context =
        new BusinessUnitClient(context.Binding, context.RemoteAddress)

    // This is in here and not in Internationalization domain,
    // because it returns only the currencies that are defined in the organization.
    let createCurrencyClient context =
        new CurrencyClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Users domain
    // ----------------------------------------

    let createAccessRightsClient context =
        new AccessRightsClient(context.Binding, context.RemoteAddress)

    let createEmploymentClient context =
        new EmploymentClient(context.Binding, context.RemoteAddress)

    let createUserClient context =
        new UserClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Customers domain
    // ----------------------------------------

    let createAccountClient context =
        new AccountClient(context.Binding, context.RemoteAddress)

    let createAccountGroupClient context =
        new AccountGroupClient(context.Binding, context.RemoteAddress)

    let createAddressClient context =
        new AddressClient(context.Binding, context.RemoteAddress)

    let createCompanyClient context =
        new CompanyClient(context.Binding, context.RemoteAddress)

    let createContactClient context =
        new ContactClient(context.Binding, context.RemoteAddress)

    let createCustomerClient context =
        new CustomerClient(context.Binding, context.RemoteAddress)

    let createIndustryClient context =
        new IndustryClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Activities domain
    // ----------------------------------------

    let createActivityClient context =
        new ActivityClient(context.Binding, context.RemoteAddress)

    let createActivityTypeClient context =
        new ActivityTypeClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Products domain
    // ----------------------------------------

    let createPricelistClient context =
        new PricelistClient(context.Binding, context.RemoteAddress)

    let createProductClient context =
        new ProductClient(context.Binding, context.RemoteAddress)

    let createProductCategoryClient context =
        new ProductCategoryClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Projects domain
    // ----------------------------------------

    let createCaseClient context =
        new CaseClient(context.Binding, context.RemoteAddress)

    let createCaseStatusClient context =
        new CaseStatusClient(context.Binding, context.RemoteAddress)

    let createLeadSourceClient context =
        new LeadSourceClient(context.Binding, context.RemoteAddress)

    let createPhaseClient context =
        new PhaseClient(context.Binding, context.RemoteAddress)

    let createPhaseMemberClient context =
        new PhaseMemberClient(context.Binding, context.RemoteAddress)

    let createSalesProcessClient context =
        new SalesProcessClient(context.Binding, context.RemoteAddress)

    let createSalesStatusClient context =
        new SalesStatusClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Resourcing domain
    // ----------------------------------------

    let createResourceClient context =
        new ResourceClient(context.Binding, context.RemoteAddress)

    let createResourceAllocationClient context =
        new ResourceAllocationClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Travel reimbursement domain
    // ----------------------------------------

    let createTravelReimbursementClient context =
        new TravelReimbursementClient(context.Binding, context.RemoteAddress)

    let createTravelReimbursementStatusClient context =
        new TravelReimbursementStatusClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Expenses and fees domain (other than work hours)
    // ----------------------------------------

    let createItemClient context =
        new ItemClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Work hours domain
    // ----------------------------------------

    let createHourEntryClient context =
        new HourEntryClient(context.Binding, context.RemoteAddress)

    let createOvertimeClient context =
        new OvertimeClient(context.Binding, context.RemoteAddress)

    let createTimeEntryClient context =
        new TimeEntryClient(context.Binding, context.RemoteAddress)

    let createWorkTypeClient context =
        new WorkTypeClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Invoicing domain
    // ----------------------------------------

    let createCostCenterClient context =
        new CostCenterClient(context.Binding, context.RemoteAddress)

    let createInvoiceClient context =
        new InvoiceClient(context.Binding, context.RemoteAddress)

    let createInvoiceStatusClient context =
        new InvoiceStatusClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Extranet domain
    // ----------------------------------------

    let createExtranetClient context =
        new ExtranetClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Multiple domains will share these features.
    // This shows that Severa does not have clear domain boundaries.
    // ----------------------------------------

    let createFileClient context =
        new FileClient(context.Binding, context.RemoteAddress)

    let createTagClient context =
        new TagClient(context.Binding, context.RemoteAddress)

    // ----------------------------------------
    // Technical domain
    // ----------------------------------------

    let createAPIClient context =
        new APIClient(context.Binding, context.RemoteAddress)

    let createHeartbeatClient context =
        new HeartbeatClient(context.Binding, context.RemoteAddress)
