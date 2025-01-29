using System.Diagnostics;

namespace pyRevit.Installer.Utils;

internal static class PyRevitUtils
{
    internal static void AttachPyRevitToRevitVersions(string pyRevitVersion, string[] revitYears)
    {
        foreach (var revitYear in revitYears)
        {
            string revitAddinPath = $@"C:\ProgramData\Autodesk\Revit\Addins\{revitYear}";

            if (Directory.Exists(revitAddinPath))
            {
                string attachCommand = pyRevitVersion == "pyRevit-5"
                    ? $"{Consts.Pyrevit5Exe} attach {pyRevitVersion} default {revitYear}"
                    : $"{Consts.Pyrevit4Exe} attach {pyRevitVersion} default {revitYear}";

                RunCommand(attachCommand, $"Failed to attach pyRevit {pyRevitVersion} to Revit {revitYear}");
            }
            else
            {
                Console.WriteLine($"Revit version {revitYear} is not installed.");
            }
        }
    }

    internal static bool IsCommandAvailable(string commandToCheck)
    {
        try
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c {commandToCheck} --help",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            using var process = Process.Start(processInfo);
            return true;
        }
        catch
        {
            return false;
        }
    }

    internal static void RunCommand(string commandToRun, string failureMessage)
    {
        try
        {
            Console.WriteLine($"Executing command: {commandToRun}");

            var processInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {commandToRun}",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = new Process { StartInfo = processInfo };
            process.Start();

            string commandOutput = process.StandardOutput.ReadToEnd();
            string commandError = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                Console.WriteLine($"{failureMessage}: {commandError}");
            }
            else if (!string.IsNullOrWhiteSpace(commandOutput))
            {
                Console.WriteLine($"Command Output: {commandOutput}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{failureMessage}. Exception: {ex.Message}");
        }
    }
}
