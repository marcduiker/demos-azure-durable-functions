# demos-azure-durable-functions

This repo contains a Function App which is part of a blog post series about Azure Durable Functions and it also serves as demo material some for my presentations.

## Dependency on 3rd party APIs

The `MeetupTravelInfo` and `FindClosestMeetups` orchestration functions depend on [Meetup](https://www.meetup.com/meetup_api/) and [Google Directions](https://developers.google.com/maps/documentation/directions/start) APIs. 

You will need API keys in order to get these functions running.

1. Obtain Meetup API keys [here](https://secure.meetup.com/meetup_api/key/).
2. Obtain Google Directions API keys [here](https://developers.google.com/maps/documentation/directions/get-api-key).
3. Find and replace `%GOOGLE_API_KEY%` and
`%MEETUP_API_KEY%` with the obtained API keys in `local.settings.json` in order to run & debug the orchestration functions locally. (You should also replace them in the http files located in the `public apis` folder if you want to directly interact with the Meetup of Google APIs).

## Azure Ops

The Azure Ops functions require a service principal. Info how to create one can be found here:
https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal


## Using the VS Code REST Client

I strongly recommend to the excellent [REST client](https://github.com/Huachao/vscode-restclient) for VS Code so you can use the http files located in the `api tests` folder to start the orchestration functions (and make calls to the Meetup and Google Directions APIs).

## My blog posts with some more info about Durable Functions.

[Azure Durable Functions - Stateful function orchestrations (part 1)](http://blog.marcduiker.nl/2017/11/05/durable-azure-functions-stateful-orchestrations.html)

[Azure Durable Functions - Stateful function orchestrations (part 2)](http://blog.marcduiker.nl/2017/11/07/durable-azure-functions-stateful-orchestrations-part2.html)