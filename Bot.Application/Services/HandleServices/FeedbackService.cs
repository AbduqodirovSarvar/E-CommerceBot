using Bot.Application.Interfaces;
using Bot.Application.Services.KeyboardServices;
using Bot.Application.Services.StateManagement;
using Bot.Domain.Entities;
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
        public FeedbackService(
            ITelegramBotClient client)
        {
            _client = client;
        }
        public async Task CatchMessage(Message message, User user, string state, CancellationToken cancellationToken)
        {
            await ForwardToAdmins(new List<long> { 636809820 }, message, user, cancellationToken);
            await StateService.Delete(message.Chat.Id);
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
                await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: AllTexts.Messages[5, (int)user.Language],
                    cancellationToken: cancellationToken);

                //await _menuButtonServices.ShowMainMenu(message, user, cancellationToken);
            }
            return;
        }
    }
}
