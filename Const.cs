namespace pyRevit.Installer;



internal static class Consts
{
    internal static readonly string PyRevitRoot;
    internal static readonly string[] PreviousInstallPaths;
    internal static readonly string EmbeddedInstallerPyrevit4;
    internal static readonly string EmbeddedInstallerPyrevit5;
    internal static readonly string Pyrevit4Exe;
    internal static readonly string Pyrevit5Exe;
    internal static readonly string[] PyRevitFrameworkYears;
    internal static readonly string[] PyRevitCoreYears;
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

        PyRevitRoot = GetDictValue(dictionary, "PyRevitRoot");
        PreviousInstallPaths =  GetDictValueAsArray(dictionary, "PreviousInstallPaths") ;
        Pyrevit4Exe = GetDictValue(dictionary, "Pyrevit4Exe");
        Pyrevit5Exe = GetDictValue(dictionary,"Pyrevit5Exe");
        PyRevit4InstallPath = GetDictValue(dictionary, "PyRevit4InstallPath");
        PyRevit5InstallPath = GetDictValue(dictionary, "PyRevit5InstallPath");
        PyRevitCoreYears = GetDictValueAsArray(dictionary, "PyRevitCoreYears");
        PyRevitFrameworkYears = GetDictValueAsArray(dictionary, "PyRevitFrameworkYears");
        EmbeddedInstallerPyrevit4 = "pyRevit.Installer.Resources.pyRevit_4*_signed.exe";
        EmbeddedInstallerPyrevit5 = "pyRevit.Installer.Resources.pyRevit_5*_signed.exe";
        AdditionalExtensionSearchPaths = GetDictValueAsArray(dictionary, "AdditionalExtensionSearchPaths");
    }

    private static string GetDictValue(Dictionary<string, string> dictionary, string key)
    {
        return dictionary.ContainsKey(key) ? dictionary[key] : throw new KeyNotFoundException($"Key '{key}' not found in the dictionary.");
    }

    private static string[] GetDictValueAsArray(Dictionary<string, string> dictionary, string key)
    {
        List<string> results = new List<string>();
        foreach (string myValue in dictionary.ContainsKey(key) ? dictionary[key].Split(',') : Array.Empty<string>())
        {
            results.Add(myValue.Trim());
        }
        return results.ToArray();


    }


}
