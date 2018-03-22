using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace build2017
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration config,
            IHostingEnvironment hostingEnvironment,
            ILogger<Startup> logger)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _logger.LogInformation("You are now in the Startup ctor()");
            _logger.LogInformation(config["urls"]);
            _logger.LogInformation("{loglevel}",config["Logging:LogLevel:Default"]);
            _logger.LogInformation("MyAppSetting value is: {0}",config["MyAppSetting"]);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // to Add ASP.NET MVC!
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                // Serve static files from images
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/MyImages",

                // set cache control for when returning static images
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }
            });

            // inline middleware
            // if the following block is un-commented
            // then the app will always just say "Hello, .NET Conf!"
            //app.Use(async (context, next) =>
            //{
            //    context.Response.WriteAsync("Hello, .NET Conf!");
            //    return;
            //});

            // conditional middleware
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/conf")
                {
                    await context.Response.WriteAsync("Hello, .NET Conf!");
                }
                else
                {
                    await next();
                }
                return;
            });

            // Allow directory browsing against /MyImages
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/MyImages"
            });

            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    //await context.Response.WriteAsync("Hello World!");
            //    await context.Response.WriteAsync(_config["Message"]);
            //});
        }
    }
}
