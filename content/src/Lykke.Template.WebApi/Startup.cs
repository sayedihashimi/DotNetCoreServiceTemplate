#if (autofac)
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
#endif
using Common.Log;
using Lykke.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lykke.Template.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                // Get configurations from settings URL, for more details:
                // https://github.com/LykkeCity/DotNetCoreServiceTemplate
                .AddFromConfiguredUrl("TEMPLATE_API_SETTINGS_URL")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public static IHostingEnvironment Environment { get; private set; }
        public static IConfigurationRoot Configuration { get; private set; }

#if (autofac)
        public IContainer ApplicationContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
#else
        public void ConfigureServices(IServiceCollection services)
#endif
        {
            // Add MVC
            services.AddMvc();

            // Create a logger
            var logger = new LogToConsole();

            // Configure API Authentication
            var apiAzureConfig = Configuration
                .GetSection("LykkeApiAuth")
                .Get<ApiAuthAzureConfig>();
            services.AddLykkeApiAuthAzure(apiAzureConfig, logger);

            // Configure services/repositories
            services.AddLykkeTemplateAzureRepositories(conf =>
            {
                conf.ConnectionString = Configuration
                    .GetValue<string>("LykkeTempApi:ConnectionString");

                conf.Logger = logger;
            });
#if (autofac)

            // Configure AutoFac
            var builder = new ContainerBuilder();
            builder.Populate(services);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
#endif
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole();
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // Use API Authentication
            app.UseLykkeApiAuth(conf =>
                conf.ApiId = Configuration.GetValue<string>("LykkeTempApi:ApiId"));

            // Use MVC
            app.UseMvc();
#if (autofac)

            // Dispose resources that have been resolved in the application container
            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
#endif
        }
    }
}
