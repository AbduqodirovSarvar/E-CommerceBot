using Bot.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot.Application.Services.HandleServices
{
    public class SettingsService : ISettingService
    {
        private readonly IAppDbContext _context;
        private readonly IMainMenuService _menuService;
        public SettingsService(
            IAppDbContext context, 
            IMainMenuService mainMenuService)
        {
             _context = context;
            _menuService = mainMenuService;
        }
        public async Task CatchMessage(Message message, string state, CancellationToken cancellationToken)
        {
            var forward = state switch
            {
                "setting" => ReceivedMessageFromSettingState(message, state, cancellationToken),
                "setting:changename" => ReceivedNewName(message, state, cancellationToken),
                "setting:changephone" => ReceivedNewPhone(message, state, cancellationToken),
                "setting:changelanguage" => ReceivedNewLanguage(message, state, cancellationToken),
                _ => ShowSettingsMenu(message, state, cancellationToken),
            };
            await forward;
            return;
        }

        private async Task ReceivedMessageFromSettingState(Message message, string state, CancellationToken cancellationToken)
        {
            var forward = message.Text! switch
            {
                "Ism familiyani o'zgartirish" => ClickChangeNameButton(message, state, cancellationToken),
                "Tilni o'zgartirish" => ClickChangePhoneButton(message, state, cancellationToken),
                "Raqamni o'zgartirish" => ClickChangePhoneButton(message, state, cancellationToken),
                "Orqaga" => ClickBackButton(message, state, cancellationToken),
                _ => ShowSettingsMenu(message, state, cancellationToken)
            };
            await forward;
            return;
        }

        private async Task ClickBackButton(Message message, string state, CancellationToken cancellationToken)
        {
            await _menuService.ShowMainMenu(message, cancellationToken);
            return;
        }

        private Task ShowSettingsMenu(Message message, string state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task ClickChangelanguageButton(Message message, string state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task ClickChangePhoneButton(Message message, string state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task ClickChangeNameButton(Message message, string state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task ReceivedNewLanguage(Message message, string state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task ReceivedNewPhone(Message message, string state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task ReceivedNewName(Message message, string state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        
    }
}
