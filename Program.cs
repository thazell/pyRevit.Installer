using pyRevit.Installer.Utils;
using System.Configuration;



namespace pyRevit.Installer;

internal static class Program
{


    private static void Main()
    {
        Console.WriteLine("Welcome to the (Unofficial) pyRevit Installer!");
        Console.WriteLine();
        Console.WriteLine("This installer automates the process of installing pyRevit 4 and pyRevit 5 side by side.");
        Console.WriteLine();
        Console.WriteLine("WARNING!");
        Console.WriteLine("Using this installer will remove any existing pyrevit installation located here:");
        Console.WriteLine(@"C:\pyRevit-Master");
        Console.WriteLine();
        Console.WriteLine("pyRevit installations located elsewhere will NOT be removed, but you will need to attach to Revit to use these versions again");
        Console.WriteLine();

        //Console.WriteLine("Included Versions");
        //Console.WriteLine($"pyRevit 4: {}");
        //Console.WriteLine($"pyRevit 5: {pyRevit5Installer}");
        //Console.WriteLine();

        Console.WriteLine("Please choose an option:");
        Console.WriteLine();
        Console.WriteLine("1. Install both pyRevit 4 (2020-2024) and pyRevit 5 (2025)");
        Console.WriteLine("2. Install only pyRevit 5 (2020-2025)");
        Console.WriteLine("3. Exit");
        Console.WriteLine();
        Console.Write("Enter your choice (1/2/3) and press enter: ");
        Console.WriteLine();
        string? input = Console.ReadLine();
        Console.WriteLine();

        Console.Clear();
        switch (input)
        {
            case "1":

                Console.WriteLine("Installing pyRevit 4 for Revit 2020-2024 and pyRevit 5 for Revit 2025...");
                Console.WriteLine();

                EnsureDirectoriesExist();

                InstallPyRevit4And5();
                break;

            case "2":

                Console.WriteLine();
                Console.WriteLine("Installing pyRevit 5 for Revit 2020-2025...");
                Console.WriteLine();

                EnsureDirectoriesExist();

                InstallPyRevit5();
                break;

            case "3":

                Console.WriteLine();

                Console.WriteLine("Exiting the installer. Goodbye!");
                return;

            default:
                Main();
                break;
        }

        Console.WriteLine();
        Console.WriteLine("Installation completed. Press any key to exit.");
        Console.ReadKey();
    }

    private static void InstallPyRevit4And5()
    {
        Console.WriteLine($"{Consts.EmbeddedInstallerPyrevit4}");
        var pyRevit4Installers = ResourceUtils.GetMatchingResourceNames($"{Consts.EmbeddedInstallerPyrevit4}");
        foreach (var installer in pyRevit4Installers)
        {
            ResourceUtils.ExtractAndInstallResource(installer, Consts.PyRevit4InstallPath);
        }

        var pyRevit5Installers = ResourceUtils.GetMatchingResourceNames($"{Consts.EmbeddedInstallerPyrevit5}");
        foreach (var installer in pyRevit5Installers)
        {
            ResourceUtils.ExtractAndInstallResource(installer, Consts.PyRevit5InstallPath);
        }

        if (!PyRevitUtils.IsCommandAvailable(Consts.Pyrevit4Exe) ||
            !PyRevitUtils.IsCommandAvailable(Consts.Pyrevit5Exe)) return;

        Console.WriteLine();
        Console.WriteLine("Adding pyRevit-4 to clones");
        PyRevitUtils.RunCommand($"{Consts.Pyrevit4Exe} revits killall", "Failed to close all Revit processes");
        PyRevitUtils.RunCommand($"{Consts.Pyrevit4Exe} clones forget --all", "Failed to forget existing pyRevit clones");
        PyRevitUtils.RunCommand($"{Consts.Pyrevit4Exe} clones add this pyRevit-4", "Failed to add pyRevit-4 clone");
        Console.WriteLine();
        Console.WriteLine("Attaching pyRevit-4");
        PyRevitUtils.AttachPyRevitToRevitVersions("pyRevit-4", Consts.PyRevitFrameworkYears);

        Console.WriteLine();
        Console.WriteLine("Adding pyRevit-5 to clones");
        PyRevitUtils.RunCommand($"{Consts.Pyrevit5Exe} clones add this pyRevit-5", "Failed to add pyRevit-5 clone");
        Console.WriteLine();
        Console.WriteLine("Attaching pyRevit-5");
        PyRevitUtils.AttachPyRevitToRevitVersions("pyRevit-5", Consts.PyRevitCoreYears);
        Console.WriteLine();
    }

    private static void InstallPyRevit5()
    {
        var pyRevit5Installers = ResourceUtils.GetMatchingResourceNames($"{Consts.EmbeddedInstallerPyrevit5}");
        foreach (var installer in pyRevit5Installers)
        {
            ResourceUtils.ExtractAndInstallResource(installer, @"C:\pyRevit-Master\pyRevit-5");
        }

        if (!PyRevitUtils.IsCommandAvailable(Consts.Pyrevit5Exe)) return;

        Console.WriteLine();
        Console.WriteLine("Adding pyRevit-5 to clones");
        PyRevitUtils.RunCommand($"{Consts.Pyrevit5Exe} revits killall", "Failed to close all Revit processes");
        PyRevitUtils.RunCommand($"{Consts.Pyrevit5Exe} clones forget --all", "Failed to forget existing pyRevit clones");
        PyRevitUtils.RunCommand($"{Consts.Pyrevit5Exe} clones add this pyRevit-5", "Failed to add pyRevit-5 clone");
        Console.WriteLine();
        Console.WriteLine("Attaching pyRevit-5");
        PyRevitUtils.AttachPyRevitToRevitVersions("pyRevit-5", Consts.PyRevitFrameworkYears);
        PyRevitUtils.AttachPyRevitToRevitVersions("pyRevit-5", Consts.PyRevitCoreYears);
    }

    private static void EnsureDirectoriesExist()
    {
        string directoryPath = Consts.PyRevitRoot;
        string[] previousInstallPaths = Consts.PreviousInstallPaths;
        Console.WriteLine($"Cleaning installation directory: {directoryPath}");

        foreach (string previousInstallPath in previousInstallPaths)
        {
            if (Directory.Exists(previousInstallPath))
            {
                try
                {
                    Directory.Delete(previousInstallPath, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete directory {previousInstallPath}. Error: {ex.Message}");
                }
            }
        }

        if (Directory.Exists(directoryPath))
        {
            try
            {
                Directory.Delete(directoryPath, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete directory {directoryPath}. Error: {ex.Message}");
            }
        }

        try
        {
            Directory.CreateDirectory(directoryPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create directory {directoryPath}. Error: {ex.Message}");
        }
    }
}
