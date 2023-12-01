using Bot.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot.Application.Services.HandleServices
{
    public class FeedbackService : IFeedbackService
    {
        public Task CatchMessage(Message message, string state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
