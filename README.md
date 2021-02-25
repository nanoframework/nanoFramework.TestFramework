nanoFramework Unit Test platform
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoframework.TestPlatform&metric=alert_status)](https://sonarcloud.io/dashboard?id=nanoframework_nanoframework.TestPlatform) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_nanoframework.TestPlatform&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=nanoframework_nanoframework.TestPlatform) [![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) [![NuGet](https://img.shields.io/nuget/dt/nanoframework.TestPlatform.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoframework.TestPlatform/) [![#yourfirstpr](https://img.shields.io/badge/first--timers--only-friendly-blue.svg)](https://github.com/nanoframework/Home/blob/master/CONTRIBUTING.md) [![Discord](https://img.shields.io/discord/478725473862549535.svg?logo=discord&logoColor=white&label=Discord&color=7289DA)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://github.com/nanoframework/Home/blob/master/resources/logo/nanoFramework-repo-logo.png)

-----

# Welcome to the **nanoFramework** TestPlatform repository!

## Build status

| Component | Build Status | NuGet Package |
|:-|---|---|
| nanoframework.TestPlatform | [![Build Status](https://dev.azure.com/nanoframework/nanoframework.TestPlatform/_apis/build/status/nanoframework.lib-nanoframework.TestPlatform?branchName=master)](https://dev.azure.com/nanoframework/nanoframework.TestPlatform/_build/latest?definitionId=65&branchName=master) | [![NuGet](https://img.shields.io/nuget/v/nanoframework.TestPlatform.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoframework.TestPlatform/) |
| nanoframework.TestPlatform (preview) | [![Build Status](https://dev.azure.com/nanoframework/nanoframework.TestPlatform/_apis/build/status/nanoframework.lib-nanoframework.TestPlatform?branchName=develop)](https://dev.azure.com/nanoframework/nanoframework.TestPlatform/_build/latest?definitionId=65&branchName=develop) | [![](https://badgen.net/badge/NuGet/preview/D7B023?icon=https://simpleicons.now.sh/azuredevops/fff)](https://dev.azure.com/nanoframework/feed/_packaging?_a=package&feed=sandbox&package=nanoframework.TestPlatform&protocolType=NuGet&view=overview) |

## What is nanoFramework.TestPlatform

nanoFramework.TestPlatform is a Unit Test platform dedicated to nanoFramework! It has all the benefits of what you're used to when using Microsoft Test platform for .NET or XUnit or any other!

The framework includes multiple elements that are including in a single nuget!

- `nanoFramework.TestPlatform` which contains the attributes to decorate your code and the `Assert` classes to check that you're code is properly doing what's expected.
- `nanoFramework.UnitTestLauncher` which is the engine launching and managing the Unit Tests.
- `nanoFramework.TestAdapter` which is the Visual Studio Test platform adapter, allowing to have the test integration in Visual Studio.

The integration looks like that:

![test integration](assets/test-integration-vs.jpg)

And the integration will point you up to your code for successful or failed tests:

![test integration failed](assets/test-integration-vs-failed.jpg)

## Usage of nanoFramework.TestPlatform

Simply add the `nanoFramework.TestPlatform` nuget to your project and you're good to go!

![test nuget](assets/test-nuget-test-framework.jpg)

Once you'll build your project, the tests will be automatically discovered:

![test discovered](assets/test-discovered.jpg)

You can then run all the tests and you'll get the result:

![test success](assets/test-success.jpg)

To have more details on usage of the framework, please refer to the detailed [documentation here](https://docs.nanoframework.net/).

## What you'll find in this repository

This repository contains the source of the core elements. You'll find them in `sources` directory. The Visual Studio projects in the root directory will open those elements. 

It does contains a simple example which are the Unit Tests for the test Framework itself in the `poc` directory.

## Feedback and documentation

For documentation, providing feedback, issues and finding out how to contribute please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/master/CONTRIBUTORS.md).

## License

The **nanoFramework** WebServer library is licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

### .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).