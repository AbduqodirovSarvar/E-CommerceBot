using Bot.Application.Interfaces;
using Bot.Application.Services.StateManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Bot.Application.Services.HandleServices
{
    public class UpdateHander : IUpdateHandler
    {
        private readonly ILogger<UpdateHander> _logger;
        private readonly IRegisterService _registerService;
        private readonly IMainMenuService _mainMenuService;
        private readonly IOrderService _orderService;
        private readonly IFeedbackService _feedbackService;
        private readonly IContactService _contactService;
        private readonly IInformationService _informationService;
        private readonly ISettingService _settingService;
        private readonly ICheckUserService _checkUserService;
        public UpdateHander(
            ILogger<UpdateHander> logger,
            IRegisterService registerService,
            IMainMenuService mainMenuService,
            IOrderService orderService,
            IFeedbackService feedbackService,
            IContactService contactService,
            IInformationService informationService,
            ISettingService settingService,
            ICheckUserService checkUserService )
        {
            _logger = logger;
            _registerService = registerService;
            _mainMenuService = mainMenuService;
            _orderService = orderService;
            _feedbackService = feedbackService;
            _contactService = contactService;
            _informationService = informationService;
            _settingService = settingService;
            _checkUserService = checkUserService;
        }
        

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
                { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            await handler;
            return;
        }

        private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            if(!await _checkUserService.CheckUserRegistered(message.Chat.Id, cancellationToken))
            {
                await _registerService.CatchMessage(message, cancellationToken);
                return;
            }

            var userState = StateService.Get(message.Chat.Id);
            var forward = userState switch
            {
                "order" => _orderService.CatchMessage(message, userState, cancellationToken),
                "order:delivery" => _orderService.CatchMessage(message, userState, cancellationToken),
                "order:takeaway" => _orderService.CatchMessage(message, userState, cancellationToken),
                "order:delivery:address" => _orderService.CatchMessage(message, userState, cancellationToken),
                "order:delivery:confirmationaddress" => _orderService.CatchMessage(message, userState, cancellationToken),

                "feedback" => _feedbackService.CatchMessage(message, userState, cancellationToken),

                "contact" => _contactService.CatchMessage(message, userState, cancellationToken),

                "information" => _informationService.CatchMessage(message, userState, cancellationToken),
                "information:filial" => _informationService.CatchMessage(message, userState, cancellationToken),

                "setting" => _settingService.CatchMessage(message, userState, cancellationToken),
                "setting:changename" => _settingService.CatchMessage(message, userState, cancellationToken),
                "setting:changephone" => _settingService.CatchMessage(message, userState, cancellationToken),
                "setting:changelanguage" => _settingService.CatchMessage(message, userState, cancellationToken),

                _ => _mainMenuService.CatchMessage(message, cancellationToken)
            };
            await forward;
            return;
        }

        private Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
