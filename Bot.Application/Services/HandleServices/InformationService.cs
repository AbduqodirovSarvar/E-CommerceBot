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
        public async Task CatchMessage(Message message, User user, string state, CancellationToken cancellationToken)
        {
            var forward = message.Text! switch
            {
                _ => ClickFilialButton(message, user, state, cancellationToken),
            };
            await forward;
        }

        private Task ClickFilialButton(Message message, User user, string state, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }
    }
}
