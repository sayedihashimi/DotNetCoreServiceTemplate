using System;
using Lykke.Extensions.Configuration;
using Lykke.Sample;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Assets.ExampleUsage
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
            serviceCollection.AddLykkeSampleClient(
                configuration.GetValue<string>("LykkeTemplateApp:TemplateApiUrl"),
                configuration.GetValue<string>("LykkeTemplateApp:TemplateApiKey"));

            var services = serviceCollection.BuildServiceProvider();

            var assetsService = services.GetService<ISamplesRepository>();

            var input = string.Empty;
            do
            {
                switch (input)
                {
                    case "add":
                        try
                        {
                            var item = new Sample.Sample
                            {
                                Id = "sample-001", //Guid.NewGuid().ToString(),
                                Name = $"Sample {DateTime.Now.Ticks}",
                                Description = "Test sample"
                            };
                            assetsService.InsertAsync(item).GetAwaiter().GetResult();
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
                            var list = assetsService.GetAsync().GetAwaiter().GetResult();
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
                        Console.WriteLine("Use 'add' or 'list' command");
                        break;
                }

                input = Console.ReadLine();
            } while (!string.IsNullOrEmpty(input));
        }
    }
}