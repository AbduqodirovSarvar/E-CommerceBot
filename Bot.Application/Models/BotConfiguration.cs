using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Models
{
    public class BotConfiguration
    {
        public static readonly string Configuration = "BotConfiguration";
        public static readonly string RouteSection = "BotConfiguration:Route";
        public string Token { get; set; } = string.Empty;
        public string HostAddress { get; set; } = string.Empty;
        public string AdminTelegramIds { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
    }
}
