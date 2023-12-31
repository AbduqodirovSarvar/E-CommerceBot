﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using User = Bot.Domain.Entities.User;

namespace Bot.Application.Interfaces.HandleInterfaces
{
    public interface IOrderDeliveryService
    {
        Task CatchMessage(Message message, User user, string state, CancellationToken cancellationToken);
    }
}
