using Common.Log;
using Lykke.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lykke.Assets.Host
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
                .AddFromConfiguredUrl("TEMPLATE_API_SETTINGS_URL")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public static IHostingEnvironment Environment { get; private set; }
        public static IConfigurationRoot Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var log = new LogToConsole();

            var apiAzureConfig = Configuration
                .GetSection("LykkeApiAuth")
                .Get<ApiAuthAzureConfig>();
            services.AddLykkeApiAuthAzure(apiAzureConfig, log);
            
            services.AddLykkeAssetsAzure(Configuration
                .GetValue<string>("LykkeTemplateApi:ConnectionString"), log);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureTemplateAuth().GetAwaiter().GetResult();

            app.UseLykkeApiAuth(conf => conf.ApiId =
                Configuration.GetValue<string>("LykkeTemplateApi:ApiId"));

            app.UseMvc();
        }
    }
}
