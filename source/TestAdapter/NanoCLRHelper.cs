// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using CliWrap;
using CliWrap.Buffered;
using nanoFramework.TestPlatform.TestAdapter;
using Newtonsoft.Json;

namespace nanoFramework.TestAdapter
{
    internal class NanoCLRHelper
    {
        public static bool NanoClrIsInstalled { get; private set; } = false;

        public static bool PrepareNanoClr(Settings settings, LogMessenger logger)
        {
            try
            {
                if (!NanoClrIsInstalled && !InstallNanoClr(settings.UsePreviewNanoClr, logger))
                {
                    return false;
                }

                return !string.IsNullOrEmpty(settings.PathToLocalCLRInstance)
                    || UpdateNanoCLRInstance(settings.CLRVersion, settings.UsePreviewClr, logger);
            }
            catch (Exception ex)
            {
                logger.LogPanicMessage($"nanoCLR setup failed: {ex}");
                return false;
            }
        }

        public static bool InstallNanoClr(bool usePreview, LogMessenger logger)
        {
            logger.LogMessage("Install/update nanoclr tool", Settings.LoggingLevel.Verbose);

            string installedVersion = GetInstalledNanoClrVersion();

            NuGetPackage package;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "nanoFramework-TestAdapter");
                string response = client.GetStringAsync("https://api.nuget.org/v3-flatcontainer/nanoclr/index.json")
                    .GetAwaiter().GetResult();
                package = JsonConvert.DeserializeObject<NuGetPackage>(response);
            }

            string latestVersion = package?.Versions?.LastOrDefault(version => usePreview || !version.Contains("-"));
            if (string.IsNullOrEmpty(latestVersion))
            {
                logger.LogPanicMessage("No matching nanoCLR CLI version was found on NuGet.");
                return false;
            }

            if (!string.Equals(installedVersion, latestVersion, StringComparison.OrdinalIgnoreCase))
            {
                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
                {
                    BufferedCommandResult result = Cli.Wrap("dotnet")
                        .WithArguments($"tool update -g nanoclr --version {latestVersion}")
                        .WithValidation(CommandResultValidation.None)
                        .ExecuteBufferedAsync(cts.Token).Task.GetAwaiter().GetResult();

                    if (result.ExitCode != 0)
                    {
                        logger.LogPanicMessage($"Failed to install/update nanoCLR CLI. Exit code {result.ExitCode}. {result.StandardOutput} {result.StandardError}");
                        return false;
                    }
                }

                installedVersion = GetInstalledNanoClrVersion();
                if (!string.Equals(installedVersion, latestVersion, StringComparison.OrdinalIgnoreCase))
                {
                    logger.LogPanicMessage($"Expected nanoCLR CLI v{latestVersion}, but PATH resolves to v{installedVersion ?? "(not found)"}.");
                    return false;
                }
            }

            logger.LogMessage($"Running nanoCLR CLI v{latestVersion}", Settings.LoggingLevel.Detailed);
            NanoClrIsInstalled = true;
            return true;
        }

        private static string GetInstalledNanoClrVersion()
        {
            try
            {
                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                {
                    BufferedCommandResult result = Cli.Wrap("nanoclr")
                        .WithArguments("--version")
                        .WithValidation(CommandResultValidation.None)
                        .ExecuteBufferedAsync(cts.Token).Task.GetAwaiter().GetResult();

                    Match version = Regex.Match(result.StandardOutput, @"(?im)^\.NET nanoFramework nanoCLR CLI v(?<version>\d+\.\d+\.\d+(?:-[0-9A-Za-z.-]+)?)(?:\+[0-9A-Za-z.-]+)?\s*$");
                    if (result.ExitCode != 0 || !version.Success)
                    {
                        throw new InvalidOperationException($"Failed to determine the installed nanoCLR CLI version: {result.StandardOutput} {result.StandardError}");
                    }

                    return version.Groups["version"].Value;
                }
            }
            catch (Win32Exception)
            {
                return null;
            }
        }

        public static bool UpdateNanoCLRInstance(string clrVersion, bool usePreview, LogMessenger logger)
        {
            logger.LogMessage("Update nanoCLR instance", Settings.LoggingLevel.Verbose);

            string arguments = "instance --update";
            if (usePreview)
            {
                arguments += " --preview";
            }
            else if (!string.IsNullOrEmpty(clrVersion))
            {
                arguments += $" --version {clrVersion}";
            }

            using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
            {
                BufferedCommandResult result = Cli.Wrap("nanoclr")
                    .WithArguments(arguments)
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteBufferedAsync(cts.Token).Task.GetAwaiter().GetResult();

                Match version = Regex.Match(result.StandardOutput, @"(?im)^\s*(?:Updated to|Already at|At)\s+v(?<version>\d+\.\d+\.\d+\.\d+)\b");
                if (result.ExitCode != 0 || !version.Success)
                {
                    logger.LogPanicMessage($"Failed to update nanoCLR instance. Exit code {result.ExitCode}. {result.StandardOutput} {result.StandardError}");
                    return false;
                }

                logger.LogMessage($"nanoCLR instance updated to v{version.Groups["version"].Value}", Settings.LoggingLevel.Detailed);
                return true;
            }
        }

        internal class NuGetPackage
        {
            public string[] Versions { get; set; }
        }
    }
}
