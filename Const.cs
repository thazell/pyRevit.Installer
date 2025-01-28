namespace pyRevit.Installer;



internal static class Consts
{
    internal static readonly string PyRevitRoot;
    internal static readonly string[] PreviousInstallPaths;
    internal static readonly string EmbeddedInstallerPyrevit4;
    internal static readonly string EmbeddedInstallerPyrevit5;
    internal static readonly string Pyrevit4Exe;
    internal static readonly string Pyrevit5Exe;
    internal static readonly int[] PyRevitFrameworkYears;
    internal static readonly int[] PyRevitCoreYears;
    internal static string PyRevit4InstallPath;
    internal static string PyRevit5InstallPath;
    internal static readonly string[] AdditionalExtensionSearchPaths;

    static Consts()
    {
        const string fileName = "settings.ini";

        string[] allLines = File.ReadAllLines(fileName);

        Dictionary<string, string> dictionary = [];

        foreach (var line in allLines)
        {
            string[] parts = line.Split('=');

            if (parts.Length != 2)
            {
                continue;
            }

            dictionary[parts[0].Trim()] = parts[1].Trim();
        }

        PyRevitRoot = dictionary["PyRevitRoot"];
        PreviousInstallPaths = dictionary["PreviousInstallPaths"]?.Split(',') ?? Array.Empty<string>() ;
        Pyrevit4Exe = dictionary["Pyrevit4Exe"];
        Pyrevit5Exe = dictionary["Pyrevit5Exe"];
        PyRevit4InstallPath = dictionary["PyRevit4InstallPath"];
        PyRevit5InstallPath = dictionary["PyRevit5InstallPath"];
        PyRevitCoreYears = dictionary["PyRevitCoreYears"].Split(',').Select(int.Parse).ToArray();
        PyRevitFrameworkYears = dictionary["PyRevitFrameworkYears"].Split(',').Select(int.Parse).ToArray();
        EmbeddedInstallerPyrevit4 = "pyRevit.Installer.Resources.pyRevit_4*_signed.exe";
        EmbeddedInstallerPyrevit5 = "pyRevit.Installer.Resources.pyRevit_5*_signed.exe";
        AdditionalExtensionSearchPaths = dictionary["AdditionalExtensionSearchPaths"]?.Split(',') ?? Array.Empty<string>();
    }




}
