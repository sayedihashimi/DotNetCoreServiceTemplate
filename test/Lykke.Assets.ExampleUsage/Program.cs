using System;
using Lykke.Extensions.Configuration;
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
            serviceCollection.AddLykkeAssetsClient(
                configuration.GetValue<string>("LykkeTemplateApp:TemplateApiUrl"),
                configuration.GetValue<string>("LykkeTemplateApp:TemplateApiKey"));

            var services = serviceCollection.BuildServiceProvider();

            var assetsService = services.GetService<IAssetsRepository>();

            var input = string.Empty;
            do
            {
                switch (input)
                {
                    case "list":
                        var list = assetsService.GetAssetsAsync().GetAwaiter().GetResult();
                        if (list == null)
                            Console.WriteLine("Error getting assets list!");

                        else
                        {
                            Console.WriteLine("Assets:");
                            foreach (var item in list)
                            {
                                Console.WriteLine(item.Id);
                            }
                        }
                        break;

                    default:
                        Console.WriteLine("Use 'list' command");
                        break;
                }

                input = Console.ReadLine();
            } while (!string.IsNullOrEmpty(input));
        }
    }
}