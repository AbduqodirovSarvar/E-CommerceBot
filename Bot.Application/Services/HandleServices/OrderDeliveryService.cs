using Bot.Application.Interfaces.DbInterfaces;
using Bot.Application.Interfaces.HandleInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Application.Services.HandleServices
{
    public class OrderDeliveryService : IOrderDeliveryService
    {
        private readonly ITelegramBotClient _client;
        private readonly IMainMenuService _mainMenuService;
        public OrderDeliveryService(ITelegramBotClient client, IMainMenuService mainMenuService)
        {
            _client = client;
            _mainMenuService = mainMenuService;
        }

        public async Task CatchMessage(Message message, Domain.Entities.User user, string state, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Not implemented",
                cancellationToken: cancellationToken);

            await _mainMenuService.ShowMainMenu(message, user, cancellationToken);
            return;
        }
    }
}
