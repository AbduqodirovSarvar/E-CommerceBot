using Bot.Application.Models;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace Bot.Api.Services
{
    public static class DepencyInjection
    {
        public static IServiceCollection WebApiLayerServices(this IServiceCollection _services, IConfiguration _configuration)
        {
            var botConfigurationSection = _configuration.GetSection(BotConfiguration.Configuration);
            _services.Configure<BotConfiguration>(botConfigurationSection);
            var botConfiguration = botConfigurationSection.Get<BotConfiguration>();

            _services.AddHostedService<ConfigureWebhook>();

            _services.AddControllers().AddNewtonsoftJson();

            _services.AddHttpClient("e-commerce")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    BotConfiguration? botConfig = sp.GetRequiredService<IConfiguration>().GetSection(BotConfiguration.Configuration).Get<BotConfiguration>();
                    TelegramBotClientOptions options = new(botConfig.Token);
                    return new TelegramBotClient(options, httpClient);
                });

            return _services;
        }
    }
}
