using Bot.Application.Services.KeyboardServices;
using Bot.Application.Services.StateManagement;
using Bot.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = Bot.Domain.Entities.User;

namespace Bot.Application.Interfaces
{
    public interface IMainMenuService
    {
        Task CatchMessage(Message message, User user, CancellationToken cancellationToken);

        Task ShowMainMenu(Message message, User user, CancellationToken cancellationToken);

        Task ClickOrderButton(Message message, User user, CancellationToken cancellationToken);

        Task ClickFeedbackButton(Message message, User user, CancellationToken cancellationToken);

        Task ClickContactButton(Message message, User user, CancellationToken cancellationToken);

        Task CLickInformationButton(Message message, User user, CancellationToken cancellationToken);

        Task ClickSettingsButton(Message message, User user, CancellationToken cancellationToken);
    }
}
