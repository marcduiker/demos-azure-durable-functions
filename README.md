# demos-azure-durable-functions

Demo project to investigate Azure Durable Functions.

Official docs: https://azure.github.io/azure-functions-durable-extension/

Setup:

- Visual Studio 2017 15.4.2
- Azure Storage Emulator 5.2
- Azure Functions CLI Tools 1.0.6

Steps to start from scratch with a new Function App:

- Create Function App in Visual Studio.
- Update Microsoft.NET.Sdk.Functions NuGet package to 1.0.6 (If you don't do this the Microsoft.Azure.WebJobs.Extensions.DurableTask package can't be installed).
- Add Microsoft.Azure.WebJobs.Extensions.DurableTask NuGet package.
