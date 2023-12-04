using Bot.Application.Interfaces.HandleInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot.Application.Services.HandleServices
{
    public class CartService : ICartService
    {
        public Task CatchMessage(Message message, Domain.Entities.User user, string state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
