using Bot.Application.Interfaces;
using Bot.Application.Interfaces.DbInterfaces;
using Bot.Application.Interfaces.HandleInterfaces;
using Bot.Application.Interfaces.KeyboardServiceInterfaces;
using Bot.Application.Services.KeyboardServices;
using Bot.Application.Services.StateManagement;
using Bot.Domain.Enums;
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
    public class RegisterService : IRegisterService
    {
        private readonly ITelegramBotClient _client;
        private readonly IReplyKeyboardService _keyboardService;
        private readonly IAppDbContext _context;
        private readonly IMainMenuService _menuMenuService;
        public RegisterService(
            ITelegramBotClient client,
            IReplyKeyboardService replyKeyboardService,
            IAppDbContext context,
            IMainMenuService menuMenuService)
        {
            _client = client;
            _keyboardService = replyKeyboardService;
            _context = context;
            _menuMenuService = menuMenuService;
        }

        private static User? UserObject;

        public async Task CatchMessage(Message message, CancellationToken cancellationToken)
        {
            var state = StateService.Get(message.Chat.Id) switch
            {
                "register:language" => ReceivedLanguage(message, cancellationToken),
                "register:fullname" => ReceivedFullName(UserObject,message, cancellationToken),
                "register:phone" => ReceivedPhoneNumber(UserObject, message, cancellationToken),
                _ => ReceivedStartCommand(message.Chat.Id, cancellationToken),
            };

            await state;
            return;
        }

        public async Task ReceivedStartCommand(long chatId, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: chatId,
                text: $"{ReplyMessages.askChoosingLanguage[0]}\n{ReplyMessages.askChoosingLanguage[1]}\n{ReplyMessages.askChoosingLanguage[2]}",
                replyMarkup: _keyboardService.CreateKeyboardMarkup(Enum.GetNames<Language>().ToList()),
                cancellationToken: cancellationToken);

            await StateService.Set(chatId, "register:language");

            UserObject = new User()
            {
                Id = chatId,
            };
            return;
        }

        private async Task ReceivedLanguage(Message message, CancellationToken cancellationToken)
        {
            Language language = Enum.TryParse(message.Text, out Language parsedLanguage) switch
            {
                true when parsedLanguage == Language.uz => Language.uz,
                true when parsedLanguage == Language.ru => Language.ru,
                _ => Language.en
            };

            UserObject = new User()
            {
                Id = message.Chat.Id,
                Language = language
            };

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.askFullName[(int)UserObject.Language],
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "register:fullname");
            return;
        }
        
        private async Task ReceivedFullName(User? user, Message message, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                await ReceivedStartCommand(message.Chat.Id, cancellationToken);
                return;
            }

            user.FullName = message.Text!;
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.askContact[(int)user.Language],
                replyMarkup: _keyboardService.CreateContactRequestKeyboardMarkup(ReplyMessages.ShareContactButtons[(int)user.Language]),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "register:phone");
            
            return;
        }

        private async Task ReceivedPhoneNumber(User? user, Message message, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                await ReceivedStartCommand(message.Chat.Id, cancellationToken);
                return;
            }

            user.Phone = message.Contact!.PhoneNumber;

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.afterRegistered[(int)user.Language],
                cancellationToken: cancellationToken);

            await _menuMenuService.ShowMainMenu(message, user, cancellationToken);
            UserObject = null;

            return;
        }
    }
}
