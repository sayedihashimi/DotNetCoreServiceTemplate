using System;
using Lykke.Extensions.Configuration;
using Lykke.Template.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Template.ExampleUsage
{
    class Program
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once ArrangeTypeMemberModifiers
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddFromConfiguredUrl("TEMPLATE_APP_SETTINGS_URL");
            var configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLykkeTemplateWebClient(
                configuration.GetValue<string>("LykkeTempApp:TemplateApiUrl"),
                configuration.GetValue<string>("LykkeTempApp:TemplateApiKey"));

            var services = serviceCollection.BuildServiceProvider();

            var samplesService = services.GetService<ISamplesRepository>();

            var input = string.Empty;
            do
            {
                switch (input)
                {
                    case "add":
                        try
                        {
                            var item = new Sample
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = $"Template {DateTime.Now.Ticks}",
                                Description = "Test sample"
                            };
                            samplesService.InsertAsync(item).GetAwaiter().GetResult();
                            Console.WriteLine("Added sample");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;

                    case "conflict":
                        try
                        {
                            var item = new Sample
                            {
                                Id = "sample-001",
                                Name = $"Template {DateTime.Now.Ticks}",
                                Description = "Test sample"
                            };
                            samplesService.InsertAsync(item).GetAwaiter().GetResult();
                            Console.WriteLine("Added sample");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;

                    case "list":
                        try
                        {
                            var list = samplesService.GetAsync().GetAwaiter().GetResult();
                            Console.WriteLine("Samples:");
                            foreach (var item in list)
                                Console.WriteLine(item.Id);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;

                    default:
                        Console.WriteLine("Use 'add', 'conflict' or 'list' command");
                        break;
                }

                input = Console.ReadLine();
            } while (!string.IsNullOrEmpty(input));
        }
    }
}