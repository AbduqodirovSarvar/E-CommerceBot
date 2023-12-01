using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot.Application.Interfaces
{
    public interface IMainMenuService
    {
        Task ShowMainMenu(Message message, CancellationToken cancellationToken);
        Task CatchMessage(Message message, CancellationToken cancellationToken);
    }
}
