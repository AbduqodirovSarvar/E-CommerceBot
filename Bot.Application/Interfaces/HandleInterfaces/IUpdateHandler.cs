using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot.Application.Interfaces.HandleInterfaces
{
    public interface IUpdateHandler
    {
        Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);
        Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken);
    }
}
