using System;
using System.Diagnostics;
using System.Management;
using System.Threading;

namespace pyRevit.Installer
{
    internal class CheckExisting
    {
        public static void CheckExistingPS()
        {
            string productName = "pyrevit";
            string script = $@"
            $app = Get-Package -Name *{productName}* -ErrorAction SilentlyContinue
            if ($app) {{
                Write-Output ""Installed: $($app.Name)""
                $found = $true
            }} else {{
                $app = Get-WmiObject -Query ""SELECT * FROM Win32_Product WHERE Lower(Name) LIKE '{productName}%'"" -ErrorAction SilentlyContinue
                if ($app) {{
                    Write-Output ""Installed: $($app.Name)""

                }}
            }}
            ";

            Console.WriteLine("Checking for existing versions of pyrevit installed in the registry:");

            ProcessStartInfo psi = new ProcessStartInfo("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                
            };

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                // Output the result from the script to the console
                Console.Write(output);
                if (!string.IsNullOrEmpty(error))
                    Console.WriteLine("Error: " + error);

                // Check if any software was found and prompt the user
                if (output.Contains("Installed"))
                {
                    Console.WriteLine("**************************************");
                    Console.WriteLine("We recommend manually uninstalling these version from your machine before continuing");
                    Console.WriteLine("Please do that now before continuing! ");
                    Console.WriteLine("You can use 'add/remove programs' or your system administrators process for removing apps.");
                    Console.WriteLine("**************************************");
                    Console.WriteLine("Do you want to continue anyways ? (y/n)");
                    string? userResponse = Console.ReadLine();
                    if (userResponse.ToLower() == "y")
                    {
                    //continue


                    }
                    else
                    {
                        //exit script
                        Console.WriteLine("Exiting script. Please uninstall pyRevit manually before running this script again.");
                        Console.WriteLine(
                            "Press any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("No existing versions of pyRevit found. Continuing with installation...");
                }
            }
        }
    }
}
