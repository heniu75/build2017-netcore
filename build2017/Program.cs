using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace build2017
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            //var urls = string.Empty;
            //// note that the following line already caters for AppSettings.json to be used as another config store
            //var host = WebHost.CreateDefaultBuilder(args)
            //    .ConfigureAppConfiguration((context, builder) => builder.AddJsonFile("MyAppSettings.json"))
            //    .ConfigureAppConfiguration((context,builder) => builder.AddJsonFile("hosting.json"))
            //    .ConfigureLogging((context, loggerFactory) =>
            //    {
            //        // e.g. add to the console
            //        //loggerFactory.AddConsole();

            //        // e.g. add to debug
            //        //loggerFactory.AddDebug();
            //        urls = context.Configuration["urls"];

            //    })
            //    .PreferHostingUrls(true)
            //    .UseUrls(urls)
            //    .UseStartup<Startup>()
            //    .Build();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: false)
                .AddCommandLine(args)
                .Build();

            var host = WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) => builder.AddJsonFile("MyAppSettings.json"))
                .ConfigureLogging((context, loggerFactory) =>
                {
                    // e.g. add to the console
                    //loggerFactory.AddConsole();

                    // e.g. add to debug
                    //loggerFactory.AddDebug();
                })
                .UseUrls("http://*:5000")
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .Build();

            return host;
        }
    }
}
