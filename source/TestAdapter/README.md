## Debugging Test Adapter

Test Adapter it's a DLL that is loaded and executed as a child process of `vstestconsole.exe`. This makes it impossible to debug from a regular debug session in Visual Studio.
To accomplish that one needs the help of a handy VS extension and some configurations. 
It's OK to leave the debug setup and configuration as this won't affect the release build and anything else related with distributing it.

Install [Microsoft Child Process Debugging Power Tool](https://marketplace.visualstudio.com/items?itemName=vsdbgplat.MicrosoftChildProcessDebuggingPowerTool).
The configuration file for this extension is already in the repository so there is nothing else to do in regards to this.

Next open the project properties page and navigate to the `Debug` tab. 
The following adjustments are required:

1. Path to the executable. Here you have to point it to the `vstest.console.exe` executable. The exact location depends on your local setup and Visual Studio version.
1. Application arguments. Here you have to use the path to the DLL that contains the Unit Tests that you want to run. Followed by the path to the runsettings that you'll be using. The path for the test adapter should be fine as it is, because on launch, it's pointing to the bin output folder of the Test Adapter project.

To summarize, this will be use to configure a debug session in the exact same way as if you where to launch the VS Test console from a command line.

Make sure to add breakpoints wherever you need to and you're ready to hit F5!
