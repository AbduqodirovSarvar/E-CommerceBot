using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot.Application.Interfaces
{
    public interface IMenuButtonServices
    {
        Task ShowMainMenu(Message message, CancellationToken cancellationToken);
        Task ShowSettingsMenu(Message message, CancellationToken cancellationToken);
        Task ShowInformationMenu(Message message, CancellationToken cancellationToken);
        Task ShowOrderMenu(Message message, CancellationToken cancellationToken);
        Task ShowInformationFilialMenu(Message message, CancellationToken cancellationToken);
        Task ShowOrderDeliveryMenu(Message message, CancellationToken cancellationToken);
        Task ShowOrderTakeAwayMenu(Message message, CancellationToken cancellationToken);
        Task ShowOrderCartMenu(Message message, CancellationToken cancellationToken);
        Task ShowOrderProductTypesMenu(Message message, CancellationToken cancellationToken);
        Task ShowOrderOrderProductsByType(Message message, CancellationToken cancellationToken);
    }
}
