﻿using Bot.Application.Interfaces.DbInterfaces;
using Bot.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Infrastructure
{
    public static class DepencyInjections
    {
        public static IServiceCollection InfrastructureLayerServices(this IServiceCollection _services, IConfiguration _configuration)
        {
            _services.AddDbContext<AppDbContext>(options
                => options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection")));

            _services.AddScoped<IAppDbContext, AppDbContext>();
            return _services;
        }
    }
}
