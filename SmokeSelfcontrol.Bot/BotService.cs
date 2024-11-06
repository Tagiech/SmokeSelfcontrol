using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SmokeSelfControl.Bot;

public class BotService
{
    private readonly BotOptions _options;

    public BotService(IOptions<BotOptions> options)
    {
        _options = options.Value;
    }
    public async Task RunAsync()
    {
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;
        
        var bot = new TelegramBotClient(_options.ApiToken, cancellationToken: cts.Token);
        var me = await bot.GetMe(ct);
        bot.OnMessage += OnMessage;

        Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
        Console.ReadLine();
        await cts.CancelAsync();
        return;

        async Task OnMessage(Message msg, UpdateType type)
        {
            if (msg.Text is null)
            {
                return;
            }
            Console.WriteLine($"Received {type} '{msg.Text}' in {msg.Chat}");
            await bot.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}", cancellationToken: ct);
        }
    }
}