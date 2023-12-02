using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot.Application.Interfaces
{
    public interface IRegisterService
    {
        Task CatchMessage(Message message, CancellationToken cancellationToken);
        Task ReceivedStartCommand(long chatId, CancellationToken cancellationToken);
    }
}
