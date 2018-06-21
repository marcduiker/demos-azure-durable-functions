# demos-azure-durable-functions

This repo contains a Function App which is part of a blog post series about Azure Durable Functions and it also serves as demo material some for my presentations and [Youtube videos](https://www.youtube.com/playlist?list=PLoSzmz8jSD1cP3nW7lpk9sIw3cvJnSA_g).

## Update 2018-06-15

I've restructured the solution (+ upgraded to .Net Core 2) and removed the functions which depended on 3rd party APIs and required additional accounts in order to use them (e.g. Meetup, Google Directions). This code is still in the repo but will be moved to another one soon.

Now the chaining and fan-out/fan-in examples use the excellent (and free to use) [SWAPI](https://swapi.co/) which doesn't require any authentication and makes the demo easier to use for newcomers.

## Demos

The Function App solution consists of the following demos (found in seperate solution folders) which can be executed independently.

### 01-Basic

#### HelloWorld

Shows how an orchestration function ([`HelloWorld`](/src/DurableFunctions.Demo.DotNetCore/01-Basics/Orchestrations/HelloWorld.cs)) calls an activity function ([`HelloWorldActivity`](/src/DurableFunctions.Demo.DotNetCore/01-Basics/Activities/HelloWorldActivity.cs)).

Run the solution and use the requests in [HelloWorld.http](/api-tests/orchestrations/01-Basic/HelloWorld.http) to start the orchestration  locally.

#### HelloName

Shows how an orchestration function ([`HelloName`](/src/DurableFunctions.Demo.DotNetCore/01-Basics/Orchestrations/HelloName.cs)) calls an activity function ([`HelloNameActivity`](/src/DurableFunctions.Demo.DotNetCore/01-Basics/Activities/HelloWorldActivity.cs)) and passing a some data to the activity.

Run the solution and use the requests in [HelloName.http](/api-tests/orchestrations/01-Basic/HelloName.http) to start the orchestration locally.

### 02-Chaining

Shows how an orchestration function ([`GetCharacterInfo`](/src/DurableFunctions.Demo.DotNetCore/02-Chaining/Orchestrations/GetCharacterInfo.cs)) calls two activity functions in a chain ([`SearchCharacter`](src/DurableFunctions.Demo.DotNetCore/02-Chaining/Activities/SearchCharacter.cs) -> [`GetPlanet`](/src/DurableFunctions.Demo.DotNetCore/02-Chaining/Activities/GetPlanet.cs)).

The orchestration function requires a (partial) name of a Star Wars character. This character is searched in the `SearchCharacter` activity which uses the `swapi.co` API. When a character is returned the `GetPlanet` activity is called (also uses `swapi.co` again) to return the name of the home planet of the character. The full name of the character and the planet are returned from the orchestration.

Run the solution and use the requests in [GetCharacterInfo.http](/api-tests/orchestrations/02-Chaining/GetCharacterInfo.http) to start the orchestration  locally.

### 03-FanOutFanIn

Shows how an orchestration function ([`GetPlanetResidents`](/src/DurableFunctions.Demo.DotNetCore/03-FanOutFanIn/Orchestrations/GetPlanetResidents.cs)) calls activity functions using the fan-out/fan-in pattern ([`SearchPlanet`](/src/DurableFunctions.Demo.DotNetCore/03-FanOutFanIn/Activities/SearchPlanet.cs) -> n * [`GetCharacter`](/src/DurableFunctions.Demo.DotNetCore/03-FanOutFanIn/Activities/GetCharacter.cs)).

The orchestration function requires a (partial) name of a Star Wars planet. This planet is searched in the `SearchPlanet` activity which uses the `swapi.co` API. When a planet is returned the `GetCharacter` activity is called for each of the residents found in the `SearchPlanet` result. The full name of the planet and the names of the planet residents are returned from the orchestration.

Run the solution and use the requests in [GetPlanetResidents.http](/api-tests/orchestrations/03-FanOutFanIn/GetPlanetResidents.http) to start the orchestration locally.

## Unit tests

Some unit tests are added to verify the fan-out/fan-in orchestration. These tests are using xUnit, Moq (for mocking the `DurableOrchestrationContextBase`) and AutoFixture for generating testdata.

## Using the VS Code REST Client

I strongly recommend to the excellent [REST client](https://github.com/Huachao/vscode-restclient) for VS Code so you can use the http files located in the [`api-tests`](/api-tests) folder to start the orchestration functions.

## My blog posts & videos with more info about Durable Functions.

[Azure Durable Functions - Stateful function orchestrations (part 1)](http://blog.marcduiker.nl/2017/11/05/durable-azure-functions-stateful-orchestrations.html)

[Azure Durable Functions - Stateful function orchestrations (part 2)](http://blog.marcduiker.nl/2017/11/07/durable-azure-functions-stateful-orchestrations-part2.html)

[Durable Functions on YouTube (part 1)](https://blog.marcduiker.nl/2017/11/15/durable-functions-youtube-part1.html)

[Durable Functions on YouTube (part 2) - Eternal orchestrations & external events](https://blog.marcduiker.nl/2017/12/01/durable-functions-youtube-part2.html)

[Durable Functions on YouTube (part 3) - The Function Chaining Pattern](https://blog.marcduiker.nl/2018/03/06/durable-functions-youtube-part3.html)