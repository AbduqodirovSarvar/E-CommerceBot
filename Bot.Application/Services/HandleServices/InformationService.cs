using Bot.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using User = Bot.Domain.Entities.User;

namespace Bot.Application.Services.HandleServices
{
    public class InformationService : IInformationService
    {
        private readonly IAppDbContext _context;
        private readonly IInlineKeyboardService _inlineKeyboardService;
        private readonly IMainMenuService _mainMenuService;
        public InformationService(
            IAppDbContext context,
            IInlineKeyboardService inlineKeyboardService,
            IMainMenuService mainMenuService)
        {
            _context = context;
            _inlineKeyboardService = inlineKeyboardService;
            _mainMenuService = mainMenuService;
        }

        public async Task CatchMessage(Message message, User user, string state, CancellationToken cancellationToken)
        {
            await _mainMenuService.CatchMessage(message, user, cancellationToken);
            return;
        }

        public async Task CatchCallbackData(CallbackQuery query, User user, string state, CancellationToken cancellationToken)
        {
            
        }
    }
}
