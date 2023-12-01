using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using User = Bot.Domain.Entities.User;

namespace Bot.Application.Interfaces
{
    public interface IMainMenuService
    {
        Task CatchMessage(Message message, User user, CancellationToken cancellationToken);
        Task ShowMainMenu(Message message, User user, CancellationToken cancellationToken);
        Task ShowSettingsMenu(Message message, User user, CancellationToken cancellationToken);
        Task ShowInformationMenu(Message message, User user, CancellationToken cancellationToken);
        Task ShowOrderMenu(Message message, User user, CancellationToken cancellationToken);
    }
}
