using System;
using System.Diagnostics;
using Microsoft.Owin.Hosting;
using Serilog;

namespace IdentityServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .LiterateConsole(outputTemplate: "{Timestamp:HH:MM} [{Level}] ({Name:l}){NewLine} {Message}{NewLine}{Exception}")
                .CreateLogger();

            // hosting identityserver
            var baseUrl = "http://localhost:5005";
            using (WebApp.Start<Startup>(baseUrl))
            {
                Console.WriteLine($"server running on {baseUrl} ...");
                Process.Start(baseUrl);
                Console.ReadLine();
            }
        }
    }
}