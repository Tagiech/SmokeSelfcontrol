using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SmokeSelfControl.Bot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<BotOptions>(context.Configuration.GetSection("Bot"));
                services.AddSingleton<BotService>();
            })
            .Build();

        var botService = host.Services.GetRequiredService<BotService>();
        await botService.RunAsync();

        await host.RunAsync();
    }
}