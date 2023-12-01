using Bot.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot.Application.Services.HandleServices
{
    public class InformationService : IInformationService
    {
        public async Task CatchMessage(Message message, string state, CancellationToken cancellationToken)
        {
            var forward = message.Text! switch
            {
                _ => ClickFilialButton(message, state, cancellationToken),
            };
            await forward;
        }

        private Task ClickFilialButton(Message message, string state, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }
    }
}
