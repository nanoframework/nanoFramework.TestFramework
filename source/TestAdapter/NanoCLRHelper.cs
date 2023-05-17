//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using CliWrap;
using CliWrap.Buffered;
using nanoFramework.TestPlatform.TestAdapter;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net;
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

            // get installed tool version (if installed)
            var cmd = Cli.Wrap("nanoclr")
                .WithArguments("--help")
                .WithValidation(CommandResultValidation.None);

            bool performInstallUpdate = false;

            // setup cancellation token with a timeout of 10 seconds
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            try
            {
                var cliResult = cmd.ExecuteBufferedAsync(cts.Token).Task.Result;

                if (cliResult.ExitCode == 0)
                {
                    var regexResult = Regex.Match(cliResult.StandardOutput, @"(?'version'\d+\.\d+\.\d+)", RegexOptions.RightToLeft);

                    if (regexResult.Success)
                    {
                        logger.LogMessage($"Running nanoclr v{regexResult.Groups["version"].Value}", Settings.LoggingLevel.Verbose);

                        // compose version
                        Version installedVersion = new Version(regexResult.Groups[1].Value);

                        NanoClrIsInstalled = true;
                        string responseContent = null;

                        // check latest version
                        using (System.Net.WebClient client = new WebClient())
                        {
                            try
                            {
                                // Set the user agent string to identify the client.
                                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

                                // Set any additional headers, if needed.
                                client.Headers.Add("Content-Type", "application/json");

                                // Set the URL to request.
                                string url = "https://api.nuget.org/v3-flatcontainer/nanoclr/index.json";

                                // Make the HTTP request and retrieve the response.
                                responseContent = client.DownloadString(url);
                            }
                            catch (WebException e)
                            {
                                // Handle any exceptions that occurred during the request.
                                Console.WriteLine(e.Message);
                            }
                        }

                        var package = JsonConvert.DeserializeObject<NuGetPackage>(responseContent);
                        Version latestPackageVersion = new Version(package.Versions[package.Versions.Length - 1]);

                        // check if we are running the latest one
                        if (latestPackageVersion > installedVersion)
                        {
                            // need to update
                            performInstallUpdate = true;
                        }
                        else
                        {
                            logger.LogMessage($"No need to update. Running v{latestPackageVersion}",
                                              Settings.LoggingLevel.Verbose);

                            performInstallUpdate = false;
                        }
                    }
                    else
                    {
                        // something wrong with the output, can't proceed
                        logger.LogPanicMessage("Failed to parse current nanoCLR CLI version!");
                    }
                }
            }
            catch (Win32Exception)
            {
                // nanoclr doesn't seem to be installed
                performInstallUpdate = true;
                NanoClrIsInstalled = false;
            }

            if (performInstallUpdate)
            {
                cmd = Cli.Wrap("dotnet")
                .WithArguments("tool update -g nanoclr")
                .WithValidation(CommandResultValidation.None);

                // setup cancellation token with a timeout of 1 minute
                using (var cts1 = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
                {
                    var cliResult = cmd.ExecuteBufferedAsync(cts1.Token).Task.Result;

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
                            logger.LogPanicMessage($"*** Failed to install/update nanoclr *** {Environment.NewLine} {cliResult.StandardOutput}");

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
                    var regexResult = Regex.Match(cliResult.StandardOutput, @"((?>v)(?'version'\d+\.\d+\.\d+\.\d+))");

                    if (regexResult.Success)
                    {
                        logger.LogMessage(
                            $"nanoCLR instance updated to v{regexResult.Groups["version"].Value}",
                            Settings.LoggingLevel.Verbose);
                    }
                    else
                    {
                        logger.LogPanicMessage($"*** Failed to update nanoCLR instance ***");
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
        internal class NuGetPackage
        {
            public string[] Versions { get; set; }
        }
    }
}
