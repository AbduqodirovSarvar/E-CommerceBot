using Bot.Application.Interfaces;
using Bot.Application.Services.StateManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Application.Services.HandleServices
{
    public class MainMenuService : IMainMenuService
    {
        private readonly ITelegramBotClient _client;
        private readonly IReplyKeyboardService _keyboardService;
        public MainMenuService(
            ITelegramBotClient client,
            IReplyKeyboardService replyKeyboardService)
        {
            _client = client;
            _keyboardService = replyKeyboardService;
        }

        public async Task CatchMessage(Message message, CancellationToken cancellationToken)
        {
            var forward = message.Text switch
            {
                "Buyurtma" => ClickOrderButton(message, cancellationToken),
                "Fikr bildirish" => ClickFeedbackButton(message, cancellationToken),
                "Aloqa" => ClickContactButton(message, cancellationToken),
                "Ma'lumot" => CLickInformationButton(message, cancellationToken),
                "Sozlamalar" => ClickSettingsButton(message, cancellationToken),
                _ => ShowMainMenu(message, cancellationToken)
            };

            await forward;
            return;
        }

        private async Task ClickOrderButton(Message message, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Kerakli bo'limni tanlang: ",
                replyMarkup: _keyboardService.CreateKeyboardMarkup(new List<string>
                {
                    "Buyutma qilish", "Olib Ketish", "Orqaga"
                }),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "order");
            return;
        }

        private async Task ClickFeedbackButton(Message message, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Kerakli bo'limni tanlang: ",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "feedback");
            return;
        }

        private async Task ClickContactButton(Message message, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "+998 93 234 03 16",
                cancellationToken: cancellationToken);

            return;
        }

        private async Task CLickInformationButton(Message message, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Kerakli bo'limni tanlang: ",
                replyMarkup: _keyboardService.CreateKeyboardMarkup(new List<string>
                {
                    "Filial1", "Filial2", "Orqaga"
                }),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "information");
            return;
        }

        private async Task ClickSettingsButton(Message message, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Kerakli bo'limni tanlang: ",
                replyMarkup: _keyboardService.CreateKeyboardMarkup(new List<string>
                {
                    "Ism familiyani o'zgartirish", "Tilni o'zgartirish", "Raqamni o'zgartirish", "Orqaga"
                }),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "setting");
            return;
        }

        public async Task ShowMainMenu(Message message, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Asosiy menu",
                replyMarkup: _keyboardService.CreateKeyboardMarkup(new List<string>
                {
                    "Buyurtma", "Fikr bildirish", "Aloqa", "Ma'lumot", "Sozlamalar"
                }),
                cancellationToken: cancellationToken);

            await StateService.Delete(message.Chat.Id);
            return;
        }
    }
}
