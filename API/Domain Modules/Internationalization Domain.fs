namespace Mutex.Visma.Severa.SOAP.API

module Country =

    let private createClient = Factory.createCountryClient

    let get guid =
        Command.forReq createClient (fun client -> client.GetCountryByGUID(guid))

    let getByCode code =
        Command.forReq createClient (fun client -> client.GetCountryByISO(code))

    let getByName name =
        Command.forReq createClient (fun client -> client.GetCountryByName(name))

module Language =

    let private createClient = Factory.createLanguageClient

    let get guid =
        Command.forReq createClient (fun client -> client.GetLanguageByGUID(guid))

    let getByIetfTag ietfTag =
        Command.forReq createClient (fun client -> client.GetLanguageByIetfTag(ietfTag))

module Timezone =

    let private createClient = Factory.createTimezoneClient

    let getAll =
        Command.forArrayReq createClient (fun client -> client.GetAllTimezones())

    let get guid =
        Command.forReq createClient (fun client -> client.GetTimezoneByGUID(guid))
