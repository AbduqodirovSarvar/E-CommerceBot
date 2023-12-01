using Bot.Application.Interfaces;
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
        public RegisterService(
            ITelegramBotClient client,
            IReplyKeyboardService replyKeyboardService,
            IAppDbContext context)
        {
            _client = client;
            _keyboardService = replyKeyboardService;
            _context = context;
        }

        private static User? UserObject;
        public async Task CatchMessage(Message message, CancellationToken cancellationToken)
        {
            var state = StateService.Get(message.Chat.Id) switch
            {
                "register:language" => ReceivedLanguage(message, cancellationToken),
                "register:fullname" => ReceivedFullName(UserObject,message, cancellationToken),
                "register:phone" => ReceivedPhoneNumber(UserObject, message, cancellationToken),
                _ => ReceivedStartCommand(message, cancellationToken),
            };

            await state;
            return;
        }

        private async Task ReceivedStartCommand(Message message, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Assalomu aleykum, botdan foydalanish uchun tilni tanlang: ",
                replyMarkup: _keyboardService.CreateKeyboardMarkup(Enum.GetNames<Language>().ToList()),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "register:language");

            UserObject = new User()
            {
                Id = message.Chat.Id,
            };
            return;
        }

        private async Task ReceivedLanguage(Message message, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "To'liq ismingizni jo'nating : ",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "register:fullname");
            
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
            return;
        }
        
        private async Task ReceivedFullName(User? user, Message message, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                await ReceivedStartCommand(message, cancellationToken);
                return;
            }

            user.FullName = message.Text!;
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Kontagingizni yuboring: ",
                replyMarkup: _keyboardService.CreateContactRequestKeyboardMarkup("Share Contact"),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "register:phone");
            
            return;
        }

        private async Task ReceivedPhoneNumber(User? user, Message message, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                await ReceivedStartCommand(message, cancellationToken);
                return;
            }

            user.Phone = message.Contact!.PhoneNumber;

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Tabriklaymiz\nSiz muvaffaqiyatli ro'yxatdan o'tdingiz!\nKerakli bolimni tanlashingiz mumkin:",
                cancellationToken: cancellationToken);

            await StateService.Delete(user.Id);

            UserObject = null;

            return;
        }
    }
}
