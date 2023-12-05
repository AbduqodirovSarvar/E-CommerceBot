using Bot.Application.Interfaces.DbInterfaces;
using Bot.Application.Interfaces.HandleInterfaces;
using Bot.Application.Models;
using Bot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Application.Services.HandleServices
{
    public class CartService : ICartService
    {
        private readonly IAppDbContext _context;
        private readonly ITelegramBotClient _client;
        private readonly IOrderService _orderService;
        private readonly IMainMenuService _mainMenuService;
        private readonly IRedisService _redisService;
        public CartService(
            IAppDbContext context,
            ITelegramBotClient client,
            IOrderService orderService,
            IMainMenuService mainMenuService,
            IRedisService redisService)
        {
            _context = context;
            _client = client;
            _orderService = orderService;
            _mainMenuService = mainMenuService;
            _redisService = redisService;
        }

        public async Task CatchMessage(Message message, Domain.Entities.User user, string state, CancellationToken cancellationToken)
        {
            var msg = message.Text switch
            {
                "Buyurtma qilish" or "Make order" or "сделать заказ" => ClickMakeOrderButton(message, user, cancellationToken),
                "Tozalash" or "Cleaning" or "Очистка" => ClickClearButton(message, user, cancellationToken),
                _ => ClickBackButton(message, user, cancellationToken)
            };
            await msg;
            return;
        }

        private async Task ClickBackButton(Message message, Domain.Entities.User user, CancellationToken cancellationToken)
        {
            await _mainMenuService.ClickOrderButton(message, user, cancellationToken);
            return;
        }

        private async Task ClickClearButton(Message message, Domain.Entities.User user, CancellationToken cancellationToken)
        {
            await _redisService.DeleteAsync(message.Chat.Id.ToString() + "order");
            await _mainMenuService.ClickOrderButton(message, user, cancellationToken);
            return;
        }

        private async Task ClickMakeOrderButton(Message message, Domain.Entities.User user, CancellationToken cancellationToken)
        {
            var orders = (await _redisService.GetObjectAsync<UserOrders>(message.Chat.Id.ToString() + "order")).Orders;
            var msg = new StringBuilder();
            var products = _context.Products.Where(x => orders.Select(x => x.ProductId).Contains(x.Id)).ToList();

            foreach (var order in orders)
            {
                var product = products.FirstOrDefault(x => x.Id == order.ProductId);
                await _context.Orders.AddAsync(order, cancellationToken);
                msg.Append($"{product.NameUZ}\n{product.DescriptionUZ}\n{product.Price}\n{order.Amount}\nSum: {order.Amount}*{product.Price} => {order.Amount * product.Price}\n\n");
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            await _redisService.DeleteAsync(message.Chat.Id.ToString() + "order");
            await _mainMenuService.ClickOrderButton(message, user, cancellationToken);
            await _client.SendTextMessageAsync(
                                chatId: 636809820,
                                text: msg.ToString(),
                                cancellationToken: cancellationToken);

            return;
        }
    }
}
