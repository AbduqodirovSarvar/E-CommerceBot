using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Application.Interfaces
{
    public interface IInlineKeyboardService
    {
        InlineKeyboardMarkup CreateKeyboardMarkup(Dictionary<string, string> options, int? row = 2);
    }
}
