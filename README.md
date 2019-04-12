# demos-azure-durable-functions

This repo contains a .NET solution which is part of a [blog post series](https://blog.marcduiker.nl/) about Azure Durable Functions. It also serves as demo material  for some of my [presentations](https://www.slideshare.net/marcduiker) and [Youtube videos](https://www.youtube.com/playlist?list=PLoSzmz8jSD1cP3nW7lpk9sIw3cvJnSA_g).

## Solution

The solution consists of two projects:
- DurableFunctions.Demo.DotNetCore, a Function App targeted at .NET Core 2.1.
- DurableFunctions.Demo.DotNetCore.Test, an xUnit test project targeted at .NET Core 2.1.

The current version of this Function App depends on the free (and fun) external [SWAPI](https://swapi.co/) in order to demonstrate function chaining and fan-out/fan-in patterns.  This API doesn't require any authentication which makes it easier to demo and experiment with. The chaining and fan-out/fan-in functions can even run without any connection to the internet when the `SkipRemoteSwapi` setting in local.settings.json is set to `true` (it is then limited to using some hardcoded values).

## Demos

The Function App solution consists of the following demos (found in seperate solution folders) which can be executed independently.

### 00-Starters

Contains the following functions to start orchestrations:

- [`HttpStart`](/src/DurableFunctions.Demo.DotNetCore/00-Starters/HttpStart.cs)
- [`HttpStartAndWait`](/src/DurableFunctions.Demo.DotNetCore/00-Starters/HttpStartAndWait.cs)

Run the solution and use the requests in [Start.http](/api-tests/orchestrations/00-Starters/Start.http) and [StartAndWait.http](/api-tests/orchestrations/00-Starters/StartAndWait.http) to learn how orchestrations can be started over HTTP and discover how the status or result is returned.

### 10-Basics

#### HelloWorld

Shows how an orchestration function ([`HelloWorldOrchestration`](/src/DurableFunctions.Demo.DotNetCore/10-Basics/Orchestrations/HelloWorldOrchestration.cs)) calls an activity function ([`HelloWorldActivity`](/src/DurableFunctions.Demo.DotNetCore/10-Basics/Activities/HelloWorldActivity.cs)).

Run the solution and use the requests in [HelloWorld.http](/api-tests/orchestrations/10-Basics/HelloWorld.http) to start the orchestration locally.

#### HelloName

Shows how an orchestration function ([`HelloName`](/src/DurableFunctions.Demo.DotNetCore/10-Basics/Orchestrations/HelloNameOrchestration.cs)) calls an activity function ([`HelloNameActivity`](/src/DurableFunctions.Demo.DotNetCore/10-Basics/Activities/HelloWorldActivity.cs)) and passing a some data to the activity.

Run the Function App locally and use the requests in [HelloName.http](/api-tests/orchestrations/10-Basics/HelloName.http) to start the orchestration.

### 20-Chaining

Shows how an orchestration function ([`GetCharacterInfoOrchestration`](/src/DurableFunctions.Demo.DotNetCore/20-Chaining/Orchestrations/GetCharacterInfoOrchestration.cs)) calls two activity functions in a chain ([`SearchCharacterActivity`](src/DurableFunctions.Demo.DotNetCore/20-Chaining/Activities/SearchCharacterActivity.cs) -> [`GetPlanetActivity`](/src/DurableFunctions.Demo.DotNetCore/20-Chaining/Activities/GetPlanetActivity.cs)).

The orchestration function requires a (partial) name of a Star Wars character. This character is searched in the `SearchCharacter` activity which uses the `swapi.co` API. When a character is returned the `GetPlanet` activity is called (also uses `swapi.co` again) to return the name of the home planet of the character. The full name of the character and the planet are returned from the orchestration.

Run the solution and use the requests in [GetCharacterInfo.http](/api-tests/orchestrations/20-Chaining/GetCharacterInfo.http) to start the orchestration  locally.

### 30-FanOutFanIn

Shows how an orchestration function ([`GetPlanetResidentsOrchestration`](/src/DurableFunctions.Demo.DotNetCore/30-FanOutFanIn/Orchestrations/GetPlanetResidentsOrchestration.cs)) calls activity functions using the fan-out/fan-in pattern ([`SearchPlanetActivity`](/src/DurableFunctions.Demo.DotNetCore/30-FanOutFanIn/Activities/SearchPlanetActivity.cs) -> n * [`GetCharacterActivity`](/src/DurableFunctions.Demo.DotNetCore/30-FanOutFanIn/Activities/GetCharacterActivity.cs)).

The orchestration function requires a (partial) name of a Star Wars planet. This planet is searched in the `SearchPlanetActivity` activity which uses the `swapi.co` API. When a planet is returned the `GetCharacterActivity` activity is called for each of the residents found in the `SearchPlanet` result. The full name of the planet and the names of the planet residents are returned from the orchestration.

Run the solution and use the requests in [GetPlanetResidents.http](/api-tests/orchestrations/30-FanOutFanIn/GetPlanetResidents.http) to start the orchestration locally.

## Unit tests

Some unit tests are added to verify the fan-out/fan-in orchestration. These tests are using xUnit, Moq (for mocking the `DurableOrchestrationContextBase`), AutoFixture for generating testdata and FluentAssertions for... right, the fluent assertions :).

## Using the VS Code REST Client

I strongly recommend to the excellent [REST client](https://github.com/Huachao/vscode-restclient) for VS Code so you can use the http files located in the [`api-tests`](/api-tests) folder to start the orchestration functions.

## My blog posts & videos with more info about Durable Functions.

Some of these are getting outdated now...

[Azure Durable Functions - Stateful function orchestrations (part 1)](http://blog.marcduiker.nl/2017/11/05/durable-azure-functions-stateful-orchestrations.html)

[Azure Durable Functions - Stateful function orchestrations (part 2)](http://blog.marcduiker.nl/2017/11/07/durable-azure-functions-stateful-orchestrations-part2.html)

[Durable Functions on YouTube (part 1)](https://blog.marcduiker.nl/2017/11/15/durable-functions-youtube-part1.html)

[Durable Functions on YouTube (part 2) - Eternal orchestrations & external events](https://blog.marcduiker.nl/2017/12/01/durable-functions-youtube-part2.html)

[Durable Functions on YouTube (part 3) - The Function Chaining Pattern](https://blog.marcduiker.nl/2018/03/06/durable-functions-youtube-part3.html)

## Questions

Feel free to leave questions or comments here as GitHub issues so I can keep track of them.