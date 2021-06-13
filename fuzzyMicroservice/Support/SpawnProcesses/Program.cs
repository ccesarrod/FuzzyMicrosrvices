using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Diagnostics;

namespace SpawnProcesses
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();

            Console.WriteLine("The current directory is {0}", path);

            char delimiter = '\\';

            string[] paths = path.Split(delimiter);

            int parts = paths.Length;
            string runningPath = string.Empty;

            foreach (string filePath in paths)
            {
                if (filePath.ToLower() == "support")
                {
                    break;
                }

                runningPath = runningPath + filePath + @"\";


            }

            Console.WriteLine(runningPath);

            StartUpProcesses startUpProcesses = new StartUpProcesses();

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string jsonFile = $"appsettings.json";

            var configuration = new ConfigurationBuilder()             
              .AddJsonFile(jsonFile, optional: true, reloadOnChange: true)
              .SetBasePath(runningPath + @"Support\SpawnProcesses" )
              .Build();


            configuration.GetSection("StartUpProcesses").Bind(startUpProcesses);
         

            Console.WriteLine("Starting Spawn Process");

            if (startUpProcesses.CartService == true)
            {
                Console.WriteLine("Starting Cart Service");
                Process process0 = new Process();
                process0.StartInfo.CreateNoWindow = true;
                process0.StartInfo.UseShellExecute = false;
                process0.StartInfo.RedirectStandardOutput = false;
                process0.StartInfo.FileName = runningPath + @"Support\Startcartservice.bat";
                process0.StartInfo.Arguments = runningPath+ @"CartService";
                process0.Start();
           }

            if (startUpProcesses.ProductService == true)
            {
                Console.WriteLine("Starting product service");
                Process process1 = new Process();
                process1.StartInfo.CreateNoWindow = true;
                process1.StartInfo.UseShellExecute = false;
                process1.StartInfo.RedirectStandardOutput = false;
                process1.StartInfo.FileName = runningPath + @"Support\Startproductservice.bat";
                process1.StartInfo.Arguments = runningPath+ @"ProductService";
                process1.Start();

            }

            if (startUpProcesses.OrderService == true)
            {
                Console.WriteLine("Starting Order service");
                Process process2 = new Process();
                process2.StartInfo.CreateNoWindow = true;
                process2.StartInfo.UseShellExecute = false;
                process2.StartInfo.RedirectStandardOutput = false;
                process2.StartInfo.FileName = runningPath + @"Support\StartOrderservice.bat";
                process2.StartInfo.Arguments = runningPath + @"OrderServer";
                process2.Start();
            }

            if (startUpProcesses.FuzzyGetway == true)
            {
                Console.WriteLine("Starting Fuzzygetway");
                Process process3 = new Process();
                process3.StartInfo.CreateNoWindow = false;
                process3.StartInfo.UseShellExecute = false;
                process3.StartInfo.RedirectStandardOutput = false;
                process3.StartInfo.FileName = runningPath + @"Support\startfuzzygetway.bat";
                process3.StartInfo.Arguments = runningPath + @"FuzzyGetway";
                process3.Start();
            }

            if (startUpProcesses.CatalogService == true)
            {
                Console.WriteLine("Starting Catalog");

                Process process4 = new Process();
                process4.StartInfo.CreateNoWindow = false;
                process4.StartInfo.UseShellExecute = false;
                process4.StartInfo.RedirectStandardOutput = false;
                process4.StartInfo.FileName = runningPath + @"Support\startCatalogService.bat";
                process4.StartInfo.Arguments = runningPath + @"CatalogService";
                process4.Start();
            }


            if (startUpProcesses.AuthenticationService == true)
            {
                Console.WriteLine("Starting Authentication Service");

                Process process5 = new Process();
                process5.StartInfo.CreateNoWindow = true;
                process5.StartInfo.UseShellExecute = false;
                process5.StartInfo.RedirectStandardOutput = false;
                process5.StartInfo.FileName = runningPath + @"Support\startAuthenticationservice.bat";
                process5.StartInfo.Arguments = runningPath + @"AuthenticationService";
                process5.Start();
            }
            if (startUpProcesses.CustomerService == true)
            {
                Console.WriteLine("Starting Customer Service");

                Process process6 = new Process();
                process6.StartInfo.CreateNoWindow = true;
                process6.StartInfo.UseShellExecute = false;
                process6.StartInfo.RedirectStandardOutput = false;
                process6.StartInfo.FileName = runningPath + @"Support\startCustomerservice.bat";
                process6.StartInfo.Arguments = runningPath + @"CustomerService";
                process6.Start();
            }


            Console.ReadKey();
        }
    }
}
