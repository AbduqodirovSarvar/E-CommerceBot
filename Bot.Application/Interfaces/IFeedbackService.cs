using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task CatchMessage(Message message, string state, CancellationToken cancellationToken);
    }
}
