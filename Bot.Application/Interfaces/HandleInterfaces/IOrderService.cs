using Bot.Application.Services.HandleServices;
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
using User = Bot.Domain.Entities.User;

namespace Bot.Application.Interfaces.HandleInterfaces
{
    public interface IOrderService
    {
        Task CatchMessage(Message message, User user, string state, CancellationToken cancellationToken);

        Task ClickShoppingCartButton(Message message, User user, CancellationToken cancellationToken);

        Task ClickOrderTakeawayButton(Message message, User user, CancellationToken cancellationToken);

        Task ClickOrderDeliveryButton(Message message, User user, CancellationToken cancellationToken);
    }
}
