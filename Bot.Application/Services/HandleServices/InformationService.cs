using Bot.Application.Interfaces.DbInterfaces;
using Bot.Application.Interfaces.HandleInterfaces;
using Bot.Application.Interfaces.KeyboardServiceInterfaces;
using Microsoft.EntityFrameworkCore;
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
    public class InformationService : IInformationService
    {
        private readonly IAppDbContext _context;
        private readonly IInlineKeyboardService _inlineKeyboardService;
        private readonly IMainMenuService _mainMenuService;
        private readonly ITelegramBotClient _client;
        public InformationService(
            IAppDbContext context,
            IInlineKeyboardService inlineKeyboardService,
            IMainMenuService mainMenuService,
            ITelegramBotClient telegramBotClient)
        {
            _context = context;
            _inlineKeyboardService = inlineKeyboardService;
            _mainMenuService = mainMenuService;
            _client = telegramBotClient;
        }

        public async Task CatchMessage(Message message, User user, string state, CancellationToken cancellationToken)
        {
            await _mainMenuService.CatchMessage(message, user, cancellationToken);
            return;
        }

        public async Task CatchCallbackData(CallbackQuery query, User user, string state, CancellationToken cancellationToken)
        {
            var forward = query.Data switch
            {
                "next" => ReceivedNextCallbackData(query, user, state, cancellationToken),
                "back" => ReceivedBackCallbackData(query, user, state, cancellationToken),
                _ => ReceivedAnyFilialIdCallbackData(query, user, state, cancellationToken)
            };
            await forward;
            return;
        }

        private async Task ReceivedAnyFilialIdCallbackData(CallbackQuery query, User user, string state, CancellationToken cancellationToken)
        {
            var filial = await _context.Filials.FirstOrDefaultAsync(x => x.Id.ToString() == query.Data, cancellationToken);
            if(filial == null)
            {
                return;
            }
            await _client.SendTextMessageAsync(
                chatId: query.From.Id,
                text: $"Any information about filial\n{filial.Location}",
                cancellationToken: cancellationToken);
            
            return;
        }

        private Task ReceivedBackCallbackData(CallbackQuery query, User user, string state, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private Task ReceivedNextCallbackData(CallbackQuery query, User user, string state, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
