//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using CliWrap;
using CliWrap.Buffered;
using nanoFramework.TestPlatform.TestAdapter;
using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace nanoFramework.TestAdapter
{
    internal class NanoCLRHelper
    {
        /// <summary>
        /// Flag to report if nanoCLR CLI .NET tool is installed.
        /// </summary>
        public static bool NanoClrIsInstalled { get; private set; } = false;

        public static bool InstallNanoClr(LogMessenger logger)
        {
            logger.LogMessage(
                "Install/upate nanoclr tool",
                Settings.LoggingLevel.Verbose);

            var cmd = Cli.Wrap("dotnet")
                .WithArguments("tool update -g nanoclr")
                .WithValidation(CommandResultValidation.None);

            // setup cancellation token with a timeout of 1 minute
            using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
            {
                var cliResult = cmd.ExecuteBufferedAsync(cts.Token).Task.Result;

                if (cliResult.ExitCode == 0)
                {
                    // this will be either (on update): 
                    // Tool 'nanoclr' was successfully updated from version '1.0.205' to version '1.0.208'.
                    // or (update becoming reinstall with same version, if there is no new version):
                    // Tool 'nanoclr' was reinstalled with the latest stable version (version '1.0.208').
                    var regexResult = Regex.Match(cliResult.StandardOutput, @"((?>version ')(?'version'\d+\.\d+\.\d+)(?>'))");

                    if (regexResult.Success)
                    {
                        logger.LogMessage($"Install/update successful. Running v{regexResult.Groups["version"].Value}",
                                          Settings.LoggingLevel.Verbose);

                        NanoClrIsInstalled = true;
                    }
                    else
                    {
                        logger.LogPanicMessage($"*** Failed to install/update nanoclr. {cliResult.StandardOutput}.");

                        NanoClrIsInstalled = false;
                    }
                }
                else
                {
                    logger.LogPanicMessage(
                        $"Failed to install/update nanoclr. Exit code {cliResult.ExitCode}."
                        + Environment.NewLine
                        + Environment.NewLine
                        + "****************************************"
                        + Environment.NewLine
                        + "*** WON'T BE ABLE TO RUN UNITS TESTS ***"
                        + Environment.NewLine
                        + "****************************************");

                    NanoClrIsInstalled = false;
                }
            }

            // report outcome
            return NanoClrIsInstalled;
        }

        public static void UpdateNanoCLRInstance(
            string clrVersion,
            LogMessenger logger)
        {
            logger.LogMessage(
                "Upate nanoCLR instance",
                Settings.LoggingLevel.Verbose);

            var arguments = "instance --update";

            if (!string.IsNullOrEmpty(clrVersion))
            {
                arguments += $" --clrversion {clrVersion}";
            }

            var cmd = Cli.Wrap("nanoclr")
                .WithArguments(arguments)
                .WithValidation(CommandResultValidation.None);

            // setup cancellation token with a timeout of 1 minute
            using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
            {
                var cliResult = cmd.ExecuteBufferedAsync(cts.Token).Task.Result;

                if (cliResult.ExitCode == 0)
                {
                    // this will be either (on update): 
                    // Updated to v1.8.1.102
                    // or (on same version):
                    // Already at v1.8.1.102
                    var regexResult = Regex.Match(cliResult.StandardOutput, @"((?>version )(?'version'\d+\.\d+\.\d+))");

                    if (regexResult.Success)
                    {
                        logger.LogMessage($"nanoCLR instance updated to v{regexResult.Groups["version"].Value}",
                                          Settings.LoggingLevel.Verbose);
                    }
                    else
                    {
                        logger.LogPanicMessage($"*** Failed to update nanoCLR instance. {cliResult.StandardOutput}.");
                    }
                }
                else
                {
                    logger.LogMessage(
                        $"Failed to update nanoCLR instance. Exit code {cliResult.ExitCode}.",
                                          Settings.LoggingLevel.Detailed);
                }
            }
        }
    }
}
