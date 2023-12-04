using Bot.Application.Interfaces.HandleInterfaces;
using Bot.Application.Services.KeyboardServices;
using Bot.Application.Services.StateManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = Bot.Domain.Entities.User;

namespace Bot.Application.Services.HandleServices
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ITelegramBotClient _client;
        private readonly IMainMenuService _mainMenuService;
        public FeedbackService(
            ITelegramBotClient client,
            IMainMenuService mainMenuService)
        {
            _client = client;
            _mainMenuService = mainMenuService;
        }
        public async Task CatchMessage(Message message, User user, string state, CancellationToken cancellationToken)
        {
            await ForwardToAdmins(new List<long> { 636809820 }, message, user, cancellationToken);
            await _mainMenuService.ShowMainMenu(message, user, cancellationToken);

            return;
        }

        private async Task ForwardToAdmins(List<long> adminIds, Message message, User user, CancellationToken cancellationToken)
        {
            foreach(long id in adminIds)
            {
                await _client.SendTextMessageAsync(
                    chatId: id,
                    text: $"New feedback:\nFrom: {message.Chat.Id}",
                    cancellationToken: cancellationToken);
                await _client.ForwardMessageAsync(
                        chatId: message.Chat.Id,
                        fromChatId: message.Chat.Id,
                        messageId: message.MessageId,
                        cancellationToken: cancellationToken);
            }
            return;
        }
    }
}
