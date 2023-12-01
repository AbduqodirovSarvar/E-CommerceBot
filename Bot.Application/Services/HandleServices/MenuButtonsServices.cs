using Bot.Application.Interfaces;
using Bot.Application.Services.KeyboardServices;
using Bot.Application.Services.StateManagement;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
    public class MenuButtonsServices
    {
        private readonly IAppDbContext _context;
        private readonly ITelegramBotClient _client;
        private readonly IInlineKeyboardService _inlineKeyboardService;
        private readonly IReplyKeyboardService _replyKeyboardService;
        public MenuButtonsServices(
            IAppDbContext context,
            ITelegramBotClient client,
            IInlineKeyboardService inlineKeyboardService,
            IReplyKeyboardService replyKeyboardService)
        {
            _context = context;
            _client = client;
            _inlineKeyboardService = inlineKeyboardService;
            _replyKeyboardService = replyKeyboardService;
        }

        public async Task ShowInformationMenu(Message message, User user, CancellationToken cancellationToken)
        {
            var filials = await _context.Filials.ToListAsync(cancellationToken);
            Dictionary<string, string> dict = filials.ToDictionary(x => x.NameUZ, x => x.Id.ToString());

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Filailni tanlang: ",
                replyMarkup: _inlineKeyboardService.CreateKeyboardMarkup(dict),
                cancellationToken: cancellationToken);
            return;
        }

        public async Task ShowMainMenu(Message message, User user, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Asosiy menu",
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(new List<string>
                {
                    "Buyurtma", "Fikr bildirish", "Aloqa", "Ma'lumot", "Sozlamalar"
                }),
                cancellationToken: cancellationToken);

            await StateService.Delete(message.Chat.Id);
            return;
        }

        public async Task ShowOrderCartMenu(Message message, User user, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Kerakli bolimni tanlang: ",
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(new List<string>
                {
                    "Yetkazib berish", "Olib ketish", "Savatcha", "Orqaga"
                }),
                cancellationToken: cancellationToken);

            return;
        }

        public async Task ShowOrderDeliveryMenu(Message message, User user, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Kerakli bolimni tanlang: ",
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(new List<string>
                {
                    "Yetkazib berish", "Olib ketish", "Savatcha", "Orqaga"
                }),
                cancellationToken: cancellationToken);

            return;
        }

        public async Task ShowOrderMenu(Message message, User user, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Kerakli bolimni tanlang: ",
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(new List<string>
                {
                    "Yetkazib berish", "Olib ketish", "Savatcha", "Orqaga"
                }),
                cancellationToken: cancellationToken);

            return;
        }

        public async Task ShowOrderOrderProductsByType(Message message, User user, CancellationToken cancellationToken)
        {
            var products = await _context.ProductTypes.Select(x => x.NameUZ).ToListAsync(cancellationToken);

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Kerakli bolimni tanlang: ",
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(products),
                cancellationToken: cancellationToken);

            return;
        }

        public async Task ShowOrderProductTypesMenu(Message message, User user, CancellationToken cancellationToken)
        {
            var products = await _context.ProductTypes.Select(x => x.NameUZ).ToListAsync(cancellationToken);

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Kerakli bolimni tanlang: ",
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(products),
                cancellationToken: cancellationToken);

            return;
        }

        public Task ShowOrderTakeAwayMenu(Message message, User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task ShowSettingsMenu(Message message, User user, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Kerakli bo'limni tanlang: ",
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(new List<string>
                {
                    "Ism familiyani o'zgartirish", "Tilni o'zgartirish", "Raqamni o'zgartirish", "Orqaga"
                }),
                cancellationToken: cancellationToken);

            return;
        }
    }
}
