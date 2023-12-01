using Bot.Application.Interfaces;
using Bot.Application.Services.KeyboardServices;
using Bot.Application.Services.StateManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = Bot.Domain.Entities.User;

namespace Bot.Application.Services.HandleServices
{
    public class MainMenuService : IMainMenuService
    {
        private readonly ITelegramBotClient _client;
        private readonly IReplyKeyboardService _replyKeyboardService;
        public MainMenuService(
            ITelegramBotClient client,
            IReplyKeyboardService replyKeyboardService)
        {
            _client = client;
            _replyKeyboardService = replyKeyboardService;
        }

        public async Task CatchMessage(Message message, User user, CancellationToken cancellationToken)
        {
            var forward = message.Text switch
            {
                "Buyurtma qilish" or "Order" or "Заказ"=> ClickOrderButton(message, user, cancellationToken),
                "Aloqa" or "Contact" or "Контакты" => ClickContactButton(message, user, cancellationToken),
                "Ma'lumot" or "Info" or "Информация" => CLickInformationButton(message, user, cancellationToken),
                "Fikr bildirish" or "Feedback" or "Обратная связь"=> ClickFeedbackButton(message, user, cancellationToken),
                "Sozlamalar" or "Settings" or "Настройки" => ClickSettingsButton(message, user, cancellationToken),
                _ => ShowMainMenu(message, user, cancellationToken)
            };

            await forward;
            return;
        }

        private async Task ClickOrderButton(Message message, User user, CancellationToken cancellationToken)
        {
            //var buttons = AllTexts.OrderMenuButtons[(int)user.Language]
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: AllTexts.Messages[4, (int)user.Language],
              ///  replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(AllTexts.OrderMenuButtons[(int)user.Language].ToList()),///
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "order");
            return;
        }

        private async Task ClickFeedbackButton(Message message, User user, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: AllTexts.Messages[6, (int)user.Language],
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "feedback");
            return;
        }

        private async Task ClickContactButton(Message message, User user, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "+998 93 234 03 16",
                cancellationToken: cancellationToken);

            return;
        }

        private async Task CLickInformationButton(Message message, User user, CancellationToken cancellationToken)
        {
           // await _menuButtonServices.ShowInformationMenu(message, user, cancellationToken);

            await StateService.Set(message.Chat.Id, "information");
            return;
        }

        private async Task ClickSettingsButton(Message message, User user, CancellationToken cancellationToken)
        {
            //await _menuButtonServices.ShowSettingsMenu(message, user, cancellationToken);

            await StateService.Set(message.Chat.Id, "setting");
            return;
        }

        public async Task ShowMainMenu(Message message, User user, CancellationToken cancellationToken)
        {
           // await _menuButtonServices.ShowMainMenu(message, user, cancellationToken);

            await StateService.Delete(message.Chat.Id);
            return;
        }

        public Task ShowSettingsMenu(Message message, User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ShowInformationMenu(Message message, User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ShowOrderMenu(Message message, User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
