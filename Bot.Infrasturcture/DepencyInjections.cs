using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Infrasturcture
{
    public static class DepencyInjections
    {
        public static IServiceCollection InfrastructureLayerServices(this IServiceCollection _services, IConfiguration _configuration)
        {
            return _services;
        }
    }
}
