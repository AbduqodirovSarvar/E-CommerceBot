using Bot.Application.Interfaces;
using Bot.Application.Services.KeyboardServices;
using Bot.Application.Services.StateManagement;
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
    public class SettingsService : ISettingService
    {
        private readonly IAppDbContext _context;
        private readonly IRedisService _redisService;
        private readonly ITelegramBotClient _client;
        private readonly IReplyKeyboardService _replyKeyboardService;
        private readonly IMainMenuService _mainMenuService;
        private readonly IRegisterService _registerService;
        public SettingsService(
            IAppDbContext context, 
            IMainMenuService mainMenuService,
            IRedisService redisService,
            ITelegramBotClient telegramBotClient, 
            IReplyKeyboardService replyKeyboardService,
            IRegisterService registerService)
        {
             _context = context;
            _mainMenuService = mainMenuService;
            _redisService = redisService;
            _client = telegramBotClient;
            _replyKeyboardService = replyKeyboardService;
            _registerService = registerService;
        }
        public async Task CatchMessage(Message message, User user, string state, CancellationToken cancellationToken)
        {
            var forward = state switch
            {
                "setting" => ReceivedMessageFromSettingState(message, user, state, cancellationToken),
                "setting:changename" => ReceivedNewName(message, user, state, cancellationToken),
                "setting:changephone" => ReceivedNewPhone(message, user, state, cancellationToken),
                "setting:changelanguage" => ReceivedNewLanguage(message, user,state, cancellationToken),
                _ => _mainMenuService.ClickSettingsButton(message, user, cancellationToken),
            };
            await forward;
            return;
        }

        private async Task ReceivedMessageFromSettingState(Message message, User user, string state, CancellationToken cancellationToken)
        {
            var forward = message.Text! switch
            {
                "Ism-Familiyani o'zgartirish" or "Change name-surname" or "Изменить имя-фамилию" => ClickChangeNameButton(message, user, state, cancellationToken),
                "Telefon raqamni o'zgartish" or "Change phone number" or "Изменить номер телефона" => ClickChangePhoneButton(message, user, state, cancellationToken),
                "Tilni o'zgartirish" or "Change language" or "Изменить язык" => ClickChangelanguageButton(message, user, state, cancellationToken),
                "Orqaga" or "Back" or "Назад" => ClickBackButton(message, user, state, cancellationToken),
                _ => _mainMenuService.ClickSettingsButton(message, user, cancellationToken)
            };
            await forward;
            return;
        }

        private async Task ClickBackButton(Message message, User user, string state, CancellationToken cancellationToken)
        {
            await _mainMenuService.ShowMainMenu(message, user, cancellationToken);
            return;
        }

        private async Task ClickChangelanguageButton(Message message, User user, string state, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.askChoosingLanguage[(int)user.Language],
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(Enum.GetNames<Language>().ToList()),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "setting:changelanguage");
            return;
        }

        private async Task ClickChangePhoneButton(Message message, User user, string state, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.askContact[(int)user.Language],
                replyMarkup: _replyKeyboardService.CreateContactRequestKeyboardMarkup(ReplyMessages.ShareContactButtons[(int)user.Language]),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "setting:changephone");
            return;
        }

        private async Task ClickChangeNameButton(Message message, User user, string state, CancellationToken cancellationToken)
        {
            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.askFullName[(int)user.Language],
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "setting:changename");
            return;
        }

        private async Task ReceivedNewLanguage(Message message, User user, string state, CancellationToken cancellationToken)
        {
            var userObject = await _context.Users.FirstOrDefaultAsync(x => x.Id == message.Chat.Id, cancellationToken);
            if (userObject == null)
            {
                await _registerService.CatchMessage(message, cancellationToken);
                return;
            }

            userObject.Language = message.Text switch
            {
                "en" => Language.en,
                "uz" => Language.uz,
                "ru" => Language.ru,
                _ => userObject.Language,
            };

            await _context.SaveChangesAsync(cancellationToken);
            await _redisService.SetObjectAsync(userObject.Id.ToString(), userObject);

            await _mainMenuService.ClickSettingsButton(message, user, cancellationToken);
            return;
        }

        private async Task ReceivedNewPhone(Message message, User user, string state, CancellationToken cancellationToken)
        {
            var userObject = await _context.Users.FirstOrDefaultAsync(x => x.Id == message.Chat.Id, cancellationToken);
            if (userObject == null)
            {
                await _registerService.CatchMessage(message, cancellationToken);
                return;
            }
            try
            {
                userObject.Phone = message.Contact!.PhoneNumber;
            }
            catch
            {
                return;
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            await _redisService.SetObjectAsync(userObject.Id.ToString(), userObject);

            await _mainMenuService.ClickSettingsButton(message, user, cancellationToken);
            return;
        }

        private async Task ReceivedNewName(Message message, User user, string state, CancellationToken cancellationToken)
        {
            var userObject = await _context.Users.FirstOrDefaultAsync(x => x.Id == message.Chat.Id, cancellationToken);
            if (userObject == null)
            {
                await _registerService.CatchMessage(message, cancellationToken);
                return;
            }
            userObject.FullName = message.Text!;
            await _context.SaveChangesAsync(cancellationToken);
            await _redisService.SetObjectAsync(userObject.Id.ToString(), userObject);

            await _mainMenuService.ClickSettingsButton(message, user, cancellationToken);
            return;
        }

    }
}
