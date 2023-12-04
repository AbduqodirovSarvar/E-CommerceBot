using Bot.Application.Interfaces;
using Bot.Application.Interfaces.DbInterfaces;
using Bot.Application.Interfaces.HandleInterfaces;
using Bot.Application.Interfaces.KeyboardServiceInterfaces;
using Bot.Application.Services.KeyboardServices;
using Bot.Application.Services.StateManagement;
using Bot.Domain.Entities;
using Bot.Domain.Enums;
using Microsoft.EntityFrameworkCore;
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
        private readonly IAppDbContext _context;
        private readonly IInlineKeyboardService _inlineKeyboardService;
        public MainMenuService(
            ITelegramBotClient client,
            IReplyKeyboardService replyKeyboardService,
            IAppDbContext context,
            IInlineKeyboardService inlineKeyboardService)
        {
            _client = client;
            _replyKeyboardService = replyKeyboardService;
            _context = context;
            _inlineKeyboardService = inlineKeyboardService;
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

        public async Task ShowMainMenu(Message message, User user, CancellationToken cancellationToken)
        {
            var length = ReplyMessages.MainMenuButtons.GetLength(1);
            List<string> keyboardsList = Enumerable.Range(0, length)
                                              .Select(x => ReplyMessages.MainMenuButtons[(int)user.Language, x])
                                                .ToList();

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.chooseCommand[(int)user.Language],
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(keyboardsList),
                cancellationToken: cancellationToken);

            await StateService.Delete(message.Chat.Id);
            return;
        }

        public async Task ClickOrderButton(Message message, User user, CancellationToken cancellationToken)
        {
            List<string> keyboardsList = Enumerable.Range(0, ReplyMessages.OrderMenuButtons.GetLength(1))
                                              .Select(x => ReplyMessages.OrderMenuButtons[(int)user.Language, x])
                                                .ToList();

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.chooseCommand[(int)user.Language],
              replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(keyboardsList),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "order");
            return;
        }

        public async Task ClickFeedbackButton(Message message, User user, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.askFeedback[(int)user.Language],
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "feedback");
            return;
        }

        public async Task ClickContactButton(Message message, User user, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "+998 93 234 03 16",
                cancellationToken: cancellationToken);

            return;
        }

        public async Task CLickInformationButton(Message message, User user, CancellationToken cancellationToken)
        {
            var filials = await _context.Filials.ToListAsync(cancellationToken);
            Dictionary<string, string> dict1 = user.Language switch
            {
                Language.uz => filials.ToDictionary(x => x.NameUZ, x => x.Id.ToString()),
                Language.ru => filials.ToDictionary(x => x.NameRU, x => x.Id.ToString()),
                _ => filials.ToDictionary(x => x.NameEN, x => x.Id.ToString())
            };

            var dict2 = user.Language switch
            {
                Language.uz => new Dictionary<string, string> { { "Avvalgi", "back" }, { "Keyingi", "next" } },
                Language.en => new Dictionary<string, string> { { "Back", "back" }, { "Next", "next" } },
                _ => new Dictionary<string, string> { { "Бывший", "back" }, { "Следующий", "next" } },
            };


            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.chooseFilial[(int)user.Language],
                replyMarkup: _inlineKeyboardService.CreateKeyboardMarkup(dict1),
                cancellationToken: cancellationToken);

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "",
                replyMarkup: _inlineKeyboardService.CreateKeyboardMarkup(dict1),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "information");
            return;
        }

        public async Task ClickSettingsButton(Message message, User user, CancellationToken cancellationToken)
        {
            List<string> keyboardsList = Enumerable.Range(0, ReplyMessages.SettingsMenuButtons.GetLength(1))
                                              .Select(x => ReplyMessages.SettingsMenuButtons[(int)user.Language, x])
                                                .ToList();

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.chooseCommand[(int)user.Language],
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(keyboardsList),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "setting");
            return;
        }
    }
}
