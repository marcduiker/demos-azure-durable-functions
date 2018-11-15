# demos-azure-durable-functions

This repo contains a .NET solution which is part of a [blog post series](https://blog.marcduiker.nl/) about Azure Durable Functions. It also serves as demo material  for some of my [presentations](https://www.slideshare.net/marcduiker) and [Youtube videos](https://www.youtube.com/playlist?list=PLoSzmz8jSD1cP3nW7lpk9sIw3cvJnSA_g).

## Solution

The solution consists of two projects:
- DurableFunctions.Demo.DotNetCore, a Function App targeted at .NET Core 2.1.
- DurableFunctions.Demo.DotNetCore.Test, an xUnit test project targeted at .NET Core 2.1.

The current version of this Function App depends on the free (and fun) external [SWAPI](https://swapi.co/) in order to demonstrate function chaining and fan-out/fan-in patterns.  This API doesn't require any authentication which makes it easier to demo and experiment with. The chaining and fan-out/fan-in functions can even run without any connection to the internet when the `SkipRemoteSwapi` setting in local.settings.json is set to `true` (it is then limited to using some hardcoded values).

## Demos

The Function App solution consists of the following demos (found in seperate solution folders) which can be executed independently.

### 01-Basics

#### HelloWorld

Shows how an orchestration function ([`HelloWorld`](/src/DurableFunctions.Demo.DotNetCore/01-Basics/Orchestrations/HelloWorld.cs)) calls an activity function ([`HelloWorldActivity`](/src/DurableFunctions.Demo.DotNetCore/01-Basics/Activities/HelloWorldActivity.cs)).

Run the solution and use the requests in [HelloWorld.http](/api-tests/orchestrations/01-Basics/HelloWorld.http) to start the orchestration  locally.

#### HelloName

Shows how an orchestration function ([`HelloName`](/src/DurableFunctions.Demo.DotNetCore/01-Basics/Orchestrations/HelloName.cs)) calls an activity function ([`HelloNameActivity`](/src/DurableFunctions.Demo.DotNetCore/01-Basics/Activities/HelloWorldActivity.cs)) and passing a some data to the activity.

Run the Function App locally and use the requests in [HelloName.http](/api-tests/orchestrations/01-Basics/HelloName.http) to start the orchestration.

### 02-Chaining

Shows how an orchestration function ([`GetCharacterInfo`](/src/DurableFunctions.Demo.DotNetCore/02-Chaining/Orchestrations/GetCharacterInfo.cs)) calls two activity functions in a chain ([`SearchCharacter`](src/DurableFunctions.Demo.DotNetCore/02-Chaining/Activities/SearchCharacter.cs) -> [`GetPlanet`](/src/DurableFunctions.Demo.DotNetCore/02-Chaining/Activities/GetPlanet.cs)).

The orchestration function requires a (partial) name of a Star Wars character. This character is searched in the `SearchCharacter` activity which uses the `swapi.co` API. When a character is returned the `GetPlanet` activity is called (also uses `swapi.co` again) to return the name of the home planet of the character. The full name of the character and the planet are returned from the orchestration.

Run the solution and use the requests in [GetCharacterInfo.http](/api-tests/orchestrations/02-Chaining/GetCharacterInfo.http) to start the orchestration  locally.

### 03-FanOutFanIn

Shows how an orchestration function ([`GetPlanetResidents`](/src/DurableFunctions.Demo.DotNetCore/03-FanOutFanIn/Orchestrations/GetPlanetResidents.cs)) calls activity functions using the fan-out/fan-in pattern ([`SearchPlanet`](/src/DurableFunctions.Demo.DotNetCore/03-FanOutFanIn/Activities/SearchPlanet.cs) -> n * [`GetCharacter`](/src/DurableFunctions.Demo.DotNetCore/03-FanOutFanIn/Activities/GetCharacter.cs)).

The orchestration function requires a (partial) name of a Star Wars planet. This planet is searched in the `SearchPlanet` activity which uses the `swapi.co` API. When a planet is returned the `GetCharacter` activity is called for each of the residents found in the `SearchPlanet` result. The full name of the planet and the names of the planet residents are returned from the orchestration.

Run the solution and use the requests in [GetPlanetResidents.http](/api-tests/orchestrations/03-FanOutFanIn/GetPlanetResidents.http) to start the orchestration locally.

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