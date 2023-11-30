using Bot.Application.Interfaces;
using Bot.Application.Services.FileServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            _services.AddScoped<IFileService, FileService>();
            return _services;
        }
    }
}
