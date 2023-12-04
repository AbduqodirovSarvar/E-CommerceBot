using Bot.Application.Interfaces;
using Bot.Application.Interfaces.DbInterfaces;
using Bot.Application.Interfaces.HandleInterfaces;
using Bot.Application.Interfaces.KeyboardServiceInterfaces;
using Bot.Application.Models;
using Bot.Application.Services.KeyboardServices;
using Bot.Application.Services.StateManagement;
using Bot.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = Bot.Domain.Entities.User;

namespace Bot.Application.Services.HandleServices
{
    public class OrderService : IOrderService
    {
        private readonly IMainMenuService _mainMenuService;
        private readonly IAppDbContext _context;
        private readonly IReplyKeyboardService _replyKeyboardService;
        private readonly IOrderTakeawayService _orderTakeawayService;
        private readonly IOrderDeliveryService _orderDeliveryService;
        private readonly IRedisService _redisService;
        private readonly ITelegramBotClient _client;
        public OrderService(
            IMainMenuService mainMenuService,
            IAppDbContext appDbContext,
            IReplyKeyboardService replyKeyboardService,
            IOrderDeliveryService orderDeliveryService,
            IOrderTakeawayService orderTakeawayService,
            IRedisService redisService,
            ITelegramBotClient client)
        {
            _mainMenuService = mainMenuService;
            _context = appDbContext;
            _replyKeyboardService = replyKeyboardService;
            _orderDeliveryService = orderDeliveryService;
            _orderTakeawayService = orderTakeawayService;
            _client = client;
            _redisService = redisService;
        }
        public async Task CatchMessage(Message message, User user, string state, CancellationToken cancellationToken)
        {
            var forward = state switch
            {
                "order" => ReceivedCommandFromOrderState(message, user, cancellationToken),
                "order:delivery" => _orderDeliveryService.CatchMessage(message, user, state, cancellationToken),
                "order:takeaway:filial" => _orderTakeawayService.CatchMessage(message, user, state, cancellationToken),
                "order:delivery:address" => _orderDeliveryService.CatchMessage(message, user, state, cancellationToken),
                "order:delivery:confirmationaddress" => _orderDeliveryService.CatchMessage(message, user, state, cancellationToken),
                _ => _mainMenuService.ClickOrderButton(message, user, cancellationToken),
            };
            await forward;
            return;
        }

        private async Task ReceivedCommandFromOrderState(Message message, User user, CancellationToken cancellationToken)
        {
            var forward = message.Text switch
            {
                "Yetkazib berish" or "Delivery" or "Доставка" => ClickOrderDeliveryButton(message, user, cancellationToken),
                "Olib Ketish" or "Take away" or "Еда на вынос" => ClickOrderTakeawayButton(message, user, cancellationToken),
                "Savatcha" or "Shopping cart" or "Корзина" => ClickShoppingCartButton(message, user, cancellationToken),
                "Orqaga" or "Back" or "Назад" => ClickBackButton(message, user, cancellationToken),
                _ => _mainMenuService.ClickOrderButton(message, user, cancellationToken)
            };
            await forward;
            return;
        }

        public async Task ClickBackButton(Message message, User user, CancellationToken cancellationToken)
        {
            await _mainMenuService.ShowMainMenu(message, user, cancellationToken);
            return;
        }

        public async Task ClickShoppingCartButton(Message message, User user, CancellationToken cancellationToken)
        {
            var orders = (await _redisService.GetObjectAsync<UserOrders>(message.Chat.Id.ToString() + "order"))?.Orders;
            if(orders == null)
            {
                await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id, 
                    text: "empty",
                    cancellationToken: cancellationToken);
                return;
            }
            var products = _context.Products.Where(x => orders.Select(x => x.ProductId).Contains(x.Id)).ToList();

            var keyboard = (products.Select(x => user.Language == Language.uz ? x.NameUZ
                                                : user.Language == Language.en ? x.NameEN
                                                    : x.NameRU).ToList());

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.yourCart[(int)user.Language],
                cancellationToken: cancellationToken);

            var msg = new StringBuilder();
            foreach(var product in products)
            {
                int amount = orders.FirstOrDefault(x => x.ProductId == product.Id)!.Amount;
                msg.Append($"{product.NameUZ}\n{product.DescriptionUZ}\n{product.Price}\n{amount}\nSum: {amount}*{product.Price} => {amount * product.Price}\n\n");
            }

            keyboard.Add(ReplyMessages.MakeOrderButtons[(int)user.Language]);
            keyboard.Add(ReplyMessages.BackButtons[(int)user.Language]);

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: msg.ToString(),
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "cart");
            return;
        }

        public async Task ClickOrderTakeawayButton(Message message, User user, CancellationToken cancellationToken)
        {
            var filials = await _context.Filials
                            .Select(x => user.Language == Language.uz ? x.NameUZ
                                : user.Language == Language.en ? x.NameEN
                                    : x.NameRU)
                            .ToListAsync(cancellationToken);

            var backbutton = user.Language switch
            {
                Language.uz => "Orqaga",
                Language.en => "Back",
                Language.ru => "Назад",
                _ => "Back"
            };

            filials.Add(backbutton);

            var keyboard = _replyKeyboardService.CreateKeyboardMarkup(filials);

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.chooseFilial[(int)user.Language],
                replyMarkup: keyboard,
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "order:takeaway:filial");
            return;
        }

        public async Task ClickOrderDeliveryButton(Message message, User user, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup keyboard = _replyKeyboardService.CreateLocationRequestKeyboardMarkup(ReplyMessages.ShareLocationButtons[(int)user.Language]);

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.askLocation[(int)user.Language],
                replyMarkup: keyboard,
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "order:delivery:address");
            return;
        }
    }
}
