using Bot.Application.Interfaces.DbInterfaces;
using Bot.Application.Interfaces.FileServiceInterfaces;
using Bot.Application.Interfaces.HandleInterfaces;
using Bot.Application.Interfaces.KeyboardServiceInterfaces;
using Bot.Application.Models;
using Bot.Application.Services.KeyboardServices;
using Bot.Application.Services.StateManagement;
using Bot.Domain.Entities;
using Bot.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = Bot.Domain.Entities.User;

namespace Bot.Application.Services.HandleServices
{
    public class OrderTakeawayService : IOrderTakeawayService
    {
        private readonly IAppDbContext _context;
        private readonly IReplyKeyboardService _replyKeyboardService;
        private readonly IRedisService _redisService;
        private readonly ITelegramBotClient _client;
        private readonly IFileService _fileService;
        public OrderTakeawayService(
            IAppDbContext context,
            IReplyKeyboardService replyKeyboardService,
            IRedisService redisService,
            ITelegramBotClient client,
            IFileService fileService)
        {
            _context = context;
            _replyKeyboardService = replyKeyboardService;
            _redisService = redisService;
            _client = client;
            _fileService = fileService;
        }

        private static readonly string[] backButton = new string[] { "Orqaga", "Back", "Назад" };

        public async Task CatchMessage(Message message, User user, string state, CancellationToken cancellationToken)
        {
            var forward = state switch
            {
                "order:takeaway:filial" => ReceivedFilialOrBackButton(message, user, state, cancellationToken),
                "order:takeaway:filial:producttype" => ReceivedProductType(message, user, state, cancellationToken),
                "order:takeaway:filial:producttype:product" => ReceivedProduct(message, user, state, cancellationToken),
                "order:takeaway:filial:producttype:product:amount" => ReceivedProductAmount(message, user, state, cancellationToken),
                _ => ShowOrdersMenu(message, user, state, cancellationToken)
            };
            await forward;
            return;
        }

        private async Task ShowOrdersMenu(Message message, User user, string state, CancellationToken cancellationToken)
        {
            List<string> keyboardsList = Enumerable.Range(0, ReplyMessages.OrderMenuButtons.GetLength(1))
                                              .Select(x => ReplyMessages.OrderMenuButtons[(int)user.Language, x])
                                                .ToList();

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.chooseCommand[(int)user.Language],
              replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(keyboardsList),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "order");
            return;
        }

        private static List<Order> orders = new List<Order>();

        private async Task ReceivedProductAmount(Message message, User user, string state, CancellationToken cancellationToken)
        {
            var order = orders.FirstOrDefault(x => x.UserId == message.Chat.Id);
            if (order == null)
            {
                await ShowOrdersMenu(message, user, state, cancellationToken);
                return;
            }

            try
            {
                order.Amount = int.Parse(message.Text!);
            }
            catch 
            {
                return;
            }
            var userOrders = await _redisService.GetObjectAsync<UserOrders>(message.Chat.Id.ToString() + "order");
            if(userOrders == null)
            {
                userOrders = new UserOrders()
                {
                    UserId = message.Chat.Id,
                    Orders = new List<Order> { order }
                };
            }
            else
            {
                userOrders.Orders.Add(order);
            }

            await _redisService.SetObjectAsync(message.Chat.Id + "order", userOrders);

            orders.Remove(order);

            await ShowOrdersMenu(message, user, state, cancellationToken);

            return;
        }

        private async Task ReceivedProduct(Message message, User user, string state, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                                    .FirstOrDefaultAsync(x => x.NameEN == message.Text
                                        || x.NameRU == message.Text
                                            || x.NameUZ == message.Text,
                                     cancellationToken);

            var order = orders.FirstOrDefault(x => x.UserId == message.Chat.Id);
            if(order == null || product == null)
            {
                await ShowOrdersMenu(message, user, state, cancellationToken);
                return;
            }

            order.ProductId = product.Id;

            await SendInformationAboutProduct(message.Chat.Id, user, product, cancellationToken);

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: ReplyMessages.askAmount[(int)user.Language],
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "order:takeaway:filial:producttype:product:amount");
            return;
        }

        private async Task ReceivedProductType(Message message, User user, string state, CancellationToken cancellationToken)
        {
            if (message.Text == "Orqaga" || message.Text == "Back" || message.Text == "Назад")
            {
                await ShowOrdersMenu(message, user, state, cancellationToken);
                return;
            }
            var type = await _context.ProductTypes
                                .FirstOrDefaultAsync(x => x.NameEN == message.Text
                                    || x.NameRU == message.Text
                                        || x.NameUZ == message.Text,
                                 cancellationToken);
            if(type == null)
            {
                return;
            }
            var products = await _context.Products
                                    .Where(x => x.TypeId == type.Id)
                                        .Select(x => user.Language == Language.uz ? x.NameUZ
                                                    : user.Language == Language.en ? x.NameEN
                                                        : x.NameRU)
                                        .ToListAsync(cancellationToken);

            var keyboard = _replyKeyboardService.CreateKeyboardMarkup(products);

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "dbsjfj",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "order:takeaway:filial:producttype:product");
            return;
        }

        private async Task ReceivedFilialOrBackButton(Message message, User user, string state, CancellationToken cancellationToken)
        {
            if(message.Text == "Orqaga" || message.Text == "Back" || message.Text == "Назад")
            {
                await ShowOrdersMenu(message, user, state, cancellationToken);
                return;
            }
            var filial = await _context.Filials
                            .FirstOrDefaultAsync(x => x.NameEN == message.Text 
                                || x.NameRU == message.Text
                                || x.NameUZ == message.Text,
                                cancellationToken);

            if (filial == null)
            {
                await ShowOrdersMenu(message, user, state, cancellationToken);
                return;
            }

            orders.Add(new Order() { UserId = message.Chat.Id, Location = filial.Location });

            var types = await _context.ProductTypes
                                    .Select(x => user.Language == Language.uz ? x.NameUZ
                                                    : user.Language == Language.en ? x.NameEN
                                                        : x.NameRU)
                                            .ToListAsync(cancellationToken);

            types.Add(backButton[(int)user.Language]);

            await _client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "asdjnjnd",
                replyMarkup: _replyKeyboardService.CreateKeyboardMarkup(types),
                cancellationToken: cancellationToken);

            await StateService.Set(message.Chat.Id, "order:takeaway:filial:producttype");

            return;
        }

        private async Task SendInformationAboutProduct(long Id, User user, Product product, CancellationToken cancellationToken)
        {
            
            var caption = user.Language switch
            {
                Language.uz => $"Nomi: {product.NameUZ}\nTasnif: {product.DescriptionUZ}\nNarxi: {product.Price}",
                Language.en => $"Name: {product.NameUZ}\\Description: {product.DescriptionUZ}\\nPrice: {product.Price}",
                _ => $"Название: {product.NameUZ}\nКлассификация: {product.DescriptionUZ}\nЦена: {product.Price}"
            };

            using (var photoStream = new FileStream(product.ImagePath, FileMode.Open))
            {
                try
                {
                    await _client.SendPhotoAsync(
                    Id,
                    InputFile.FromStream(photoStream),
                    caption: caption,
                    cancellationToken: cancellationToken);
                }
                catch 
                {
                    await _client.SendTextMessageAsync(
                        chatId: Id,
                        text: caption,
                        cancellationToken: cancellationToken);
                }

            }
            return;
        }
    }
}
