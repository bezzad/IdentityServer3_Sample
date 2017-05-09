using Microsoft.Owin.Hosting;
using System;

namespace IdentityServer3SelfHosted
{
    class Program
    {
        static void Main()
        {
            string baseAddress = "http://localhost:9876/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpClient and make a request to api/values 
                
                Console.WriteLine($"Server start on {baseAddress}");
                Console.ReadLine();
            }
        }
    }
}
