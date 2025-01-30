[![NuGet](https://img.shields.io/nuget/dt/nanoframework.TestFramework.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoframework.TestFramework/) [![#yourfirstpr](https://img.shields.io/badge/first--timers--only-friendly-blue.svg)](https://github.com/nanoframework/Home/blob/main/CONTRIBUTING.md) [![Discord](https://img.shields.io/discord/478725473862549535.svg?logo=discord&logoColor=white&label=Discord&color=7289DA)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----

# Welcome to the .NET **nanoFramework** Unit Test Framework repository

## Build status

| Component | Build Status | NuGet Package |
|:-|---|---|
| nanoframework.TestFramework | [![Build Status](https://dev.azure.com/nanoframework/nanoframework.TestFramework/_apis/build/status/nanoFramework.TestFramework?repoName=nanoframework%2FnanoFramework.TestFramework&branchName=main)](https://dev.azure.com/nanoframework/nanoframework.TestFramework/_build/latest?definitionId=67&repoName=nanoframework%2FnanoFramework.TestFramework&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoframework.TestFramework.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoframework.TestFramework/) |

## What is the .NET **nanoFramework** Test Framework

nanoFramework TestFramework it's a Unit Test framework dedicated to .NET **nanoFramework**! It has all the benefits of what you're used to when using Microsoft Test platform for .NET or XUnit or any other!

The framework includes multiple elements that are distributed in a single NuGet package!

- `nanoFramework.TestFramework` which contains the attributes to decorate your code and the `Assert` classes to check that you're code is properly doing what's expected.
- `nanoFramework.UnitTestLauncher` which is the engine launching and managing the Unit Tests.
- `nanoFramework.TestAdapter` which is the Visual Studio Test platform adapter, allowing to have the test integration in Visual Studio.

The integration looks like that:

![test integration](assets/test-integration-vs.jpg)

And the integration will point you up to your code for successful or failed tests:

![test integration failed](assets/test-integration-vs-failed.jpg)

## Usage of .NET nanoFramework Test Framework

Simply add the `nanoFramework.TestFramework` nuget to your project and you're good to go!

![test nuget](assets/test-nuget-test-framework.jpg)

Once you'll build your project, the tests will be automatically discovered:

![test discovered](assets/test-discovered.jpg)

You can then run all the tests and you'll get the result:

![test success](assets/test-success.jpg)

To have more details on usage of the framework, please refer to the detailed [documentation here](https://docs.nanoframework.net/).

## Know limitations

.NET nanoFramework Test Framework is supported in Visual Studio versions 2022 and 2019, only. Visual Studio 2017 is not fully supported. Unit Tests can be run only from the VS Test console.

## What you'll find in this repository

This repository contains the source of the core elements. You'll find them in `sources` directory. The Visual Studio project is in the root directory will open those elements.

## Sample pack

You can find on our samples repo a [sample pack](https://github.com/nanoframework/Samples/tree/main/samples/UnitTest) with projects demoing how to use the Unit Test Framework.

## Feedback and documentation

For documentation, providing feedback, issues and finding out how to contribute please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md).

## License

The .NET **nanoFramework** Test Framework is licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behaviour in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
