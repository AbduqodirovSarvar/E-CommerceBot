﻿using Bot.Application.Interfaces;
using Bot.Application.Services.CacheServices;
using Bot.Application.Services.FileServices;
using Bot.Application.Services.HandleServices;
using Bot.Application.Services.KeyboardServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application
{
    public static class DepencyInjections
    {
        public static IServiceCollection ApplicationLayerServices(this IServiceCollection _services, IConfiguration _configuration)
        {
            _services.AddSingleton<IConnectionMultiplexer>
                (ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis") ?? "localhost"));

            _services.AddScoped<IRedisService, RedisService>();
            _services.AddScoped<IFileService, FileService>();
            _services.AddScoped<IReplyKeyboardService, ReplyKeyboardService>();
            _services.AddScoped<IInlineKeyboardService, InlineKeyboardService>();
            _services.AddScoped<IMainMenuService, MainMenuService>();
            _services.AddScoped<IOrderService, OrderService>();
            _services.AddScoped<IFeedbackService, FeedbackService>();
            _services.AddScoped<IContactService, ContactService>();
            _services.AddScoped<IInformationService, InformationService>();
            _services.AddScoped<ICheckUserService, CheckUserService>();
            return _services;
        }
    }
}
